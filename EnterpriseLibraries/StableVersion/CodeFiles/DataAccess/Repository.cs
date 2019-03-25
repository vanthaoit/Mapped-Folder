namespace LogixHealth.EnterpriseLibrary.DataAccess
{
    using Dapper;

    using System;
    using System.Linq;
    using System.Reflection;

    public static class UnitOfWorkFactory
    {
        public static IUnitOfWork CreateUnitOfWork(string connectionString)
        {
            return UnitOfWork.Create(connectionString);
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly System.Data.IDbConnection _context;

        private UnitOfWork(string connectionString)
        {
            _context = new System.Data.SqlClient.SqlConnection(connectionString);
        }

        public static UnitOfWork Create(string connectionString)
        {
            return new UnitOfWork(connectionString);
        }

        IRepository<T> IUnitOfWork.CreateRepository<T>(string schema, string table)
        {
            Repository<T>.Connection = _context;
            return Repository<T>.Create(schema, table);
        }
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, T>> _entities;
        private readonly System.Collections.Generic.IDictionary<string, dynamic> _columnMapping;

        private readonly string _schema;
        private readonly string _table;

        private const string _insert = "INSERT";
        private const string _update = "UPDATE";
        private const string _delete = "DELETE";

        private Repository(string schema, string table)
        {
            _entities = new System.Collections.ObjectModel.Collection<System.Collections.Generic.KeyValuePair<string, T>>();
            _columnMapping = MapColumns(typeof(T));

            _schema = schema;
            _table = table;
        }

        private static System.Collections.Generic.IDictionary<string, dynamic> MapColumns(Type type)
        {
            System.Collections.Generic.IDictionary<string, dynamic> columns = new System.Collections.Generic.Dictionary<string, dynamic>();
            PropertyInfo[] properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.CustomAttributes.Count() > 0)
                {
                    foreach (var attribute in property.CustomAttributes)
                    {
                        switch (attribute.AttributeType.Name)
                        {
                            case "Ignore":
                            case "IgnoreAttribute":
                                if (columns.ContainsKey(property.Name))
                                    columns.Remove(property.Name);
                                else
                                    columns.Add(property.Name, "Ignore");
                                break;

                            case "DataMember":
                            case "DataMemberAttribute":
                                if (columns.ContainsKey(property.Name) && columns[property.Name] == "Ignore")
                                    columns.Remove(property.Name);
                                else
                                    columns.Add(property.Name, property.Name);
                                break;

                            default:
                                if (attribute.ConstructorArguments.Count > 0)
                                    columns.Add(property.Name, attribute.ConstructorArguments[0].Value);
                                break;
                        }
                    }
                }
                else
                {
                    columns.Add(property.Name, property.Name);
                }
            }

            return columns;
        }

        public static System.Data.IDbConnection Connection { get; set; }

        public static Repository<T> Create(string schema, string table)
        {
            return new Repository<T>(schema, table);
        }

        System.Collections.Generic.ICollection<T> IRepository<T>.AllRecords()
        {
            using (System.Data.IDbConnection connection = Connection)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                System.Collections.Generic.IEnumerable<T> entities =
                    Dapper.SqlMapper.Query<T>
                    (
                        connection,
                        string.Format("SELECT * FROM {0}.{1} WITH (NOLOCK) ", _schema, _table)
                    );

                connection.Close();

                return entities.ToList();
            }
        }

        System.Collections.Generic.ICollection<T> IRepository<T>.FindRecords(System.Linq.Expressions.Expression<System.Func<T, bool>> where)
        {
            using (System.Data.IDbConnection connection = Connection)
            {
                string columns = BuildSelectColumns();
                string whereClause = BuildWhereClause(where.Body);
                string sqlStatement = string.Format("SELECT {0} FROM [{1}].[{2}] WITH (NOLOCK) WHERE {3} ", columns, _schema, _table, whereClause);

                System.Collections.Generic.IEnumerable<T> entities =
                    connection.Query<T>(sqlStatement);

                return (System.Collections.Generic.ICollection<T>)entities;
            }
        }

        T IRepository<T>.SingleRecord(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            using (System.Data.IDbConnection connection = Connection)
            {
                string columns = BuildSelectColumns();
                string whereClause = BuildWhereClause(where.Body);
                string sqlStatement = string.Format("SELECT {0} FROM [{1}].[{2}] WITH (NOLOCK) WHERE {3} ", columns, _schema, _table, whereClause);

                System.Collections.Generic.IEnumerable<T> entities = connection.Query<T>(sqlStatement);
                T entity = default(T);
                if (entities != null)
                    if (entities.Any())
                        entity = entities.FirstOrDefault();

                return entity;
            }
        }

        private string BuildSelectColumns()
        {
            string columns = string.Empty;

            foreach (System.Collections.Generic.KeyValuePair<string, dynamic> column in _columnMapping)
            {
                columns = string.IsNullOrEmpty(columns) ? string.Format(" [{0}]", column.Value) : columns + (string.Format(", [{0}]", column.Value));
            }

            return columns;
        }

        private string BuildWhereClause(System.Linq.Expressions.Expression expression, bool isUnary = false, bool quote = true)
        {
            #region Old Code
            //if (expression is System.Linq.Expressions.UnaryExpression unary)
            //{
            //    string right = BuildWhereClause(unary.Operand, true);

            //    return "(" + NodeTypeToString(unary.NodeType, right == "NULL") + " " + right + ")";
            //}
            #endregion
            if (expression is System.Linq.Expressions.UnaryExpression)
            {
                string right = BuildWhereClause(((System.Linq.Expressions.UnaryExpression)expression).Operand, true);

                return "(" + NodeTypeToString(((System.Linq.Expressions.UnaryExpression)expression).NodeType, right == "NULL") + " " + right + ")";
            }

            #region Old Code
            //if (expression is System.Linq.Expressions.BinaryExpression body)
            //{
            //    string right = BuildWhereClause(body.Right);

            //    return "(" + BuildWhereClause(body.Left) + " " + NodeTypeToString(body.NodeType, right == "NULL") + " " + right + ")";
            //}
            #endregion
            if (expression is System.Linq.Expressions.BinaryExpression)
            {
                string right = BuildWhereClause(((System.Linq.Expressions.BinaryExpression)expression).Right);

                return "(" + BuildWhereClause(((System.Linq.Expressions.BinaryExpression)expression).Left) + " " + NodeTypeToString(((System.Linq.Expressions.BinaryExpression)expression).NodeType, right == "NULL") + " " + right + ")";
            }

            if (expression is System.Linq.Expressions.ConstantExpression)
            {
                return ValueToString(((System.Linq.Expressions.ConstantExpression)expression).Value, isUnary, quote);
            }

            #region Old Code
            //if (expression is System.Linq.Expressions.MemberExpression member)
            //{
            //    if (member.Member is System.Reflection.PropertyInfo property)
            //    {
            //        string colName = GetColumnNameFor(property.Name);
            //        if (colName.Length > 0)
            //        {
            //            if (isUnary && member.Type == typeof(bool))
            //            {
            //                return "([" + colName + "] = 1)";
            //            }
            //            return "[" + colName + "]";
            //        }
            //    }
            //    if (member.Member is System.Reflection.FieldInfo)
            //    {
            //        return ValueToString(GetValue(member), isUnary, quote);
            //    }
            //}
            #endregion
            if (expression is System.Linq.Expressions.MemberExpression)
            {
                if (((System.Linq.Expressions.MemberExpression)expression).Member is System.Reflection.PropertyInfo)
                {
                    string colName = GetColumnNameFor(((System.Linq.Expressions.MemberExpression)expression).Member.Name);
                    if (colName.Length > 0)
                    {
                        if (isUnary && ((System.Linq.Expressions.MemberExpression)expression).Type == typeof(bool))
                        {
                            return "([" + colName + "] = 1)";
                        }
                        return "[" + colName + "]";
                    }
                }
                if (((System.Linq.Expressions.MemberExpression)expression).Member is System.Reflection.FieldInfo)
                {
                    return ValueToString(GetValue(((System.Linq.Expressions.MemberExpression)expression)), isUnary, quote);
                }
            }

            #region Old Code
            //if (expression is System.Linq.Expressions.MethodCallExpression methodCall)
            //{
            //    if (methodCall.Method == typeof(string).GetMethod("Contains", new[] { typeof(string) }))
            //    {
            //        return "(" + BuildWhereClause(methodCall.Object) + " LIKE '%" + BuildWhereClause(methodCall.Arguments[0], quote: false) + "%')";
            //    }

            //    if (methodCall.Method == typeof(string).GetMethod("StartsWith", new[] { typeof(string) }))
            //    {
            //        return "(" + BuildWhereClause(methodCall.Object) + " LIKE '" + BuildWhereClause(methodCall.Arguments[0], quote: false) + "%')";
            //    }

            //    if (methodCall.Method == typeof(string).GetMethod("EndsWith", new[] { typeof(string) }))
            //    {
            //        return "(" + BuildWhereClause(methodCall.Object) + " LIKE '%" + BuildWhereClause(methodCall.Arguments[0], quote: false) + "')";
            //    }

            //    if (methodCall.Method.Name == "Contains")
            //    {
            //        System.Linq.Expressions.Expression collection;
            //        System.Linq.Expressions.Expression property;

            //        if (methodCall.Method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute)) && methodCall.Arguments.Count == 2)
            //        {
            //            collection = methodCall.Arguments[0];
            //            property = methodCall.Arguments[1];
            //        }
            //        else if (!methodCall.Method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute)) && methodCall.Arguments.Count == 1)
            //        {
            //            collection = methodCall.Object;
            //            property = methodCall.Arguments[0];
            //        }
            //        else
            //        {
            //            throw new System.Exception("Unsupported expression");
            //        }

            //        System.Collections.IEnumerable values = (System.Collections.IEnumerable)GetValue(collection);
            //        string concated = "";

            //        foreach (var e in values)
            //        {
            //            concated += ValueToString(e, false, true) + ", ";
            //        }

            //        return concated == ""
            //            ? ValueToString(false, true, false)
            //            : "(" + BuildWhereClause(property) + " IN (" + concated.Substring(0, concated.Length - 2) + "))";
            //    }
            //}
            #endregion
            if (expression is System.Linq.Expressions.MethodCallExpression)
            {
                if (((System.Linq.Expressions.MethodCallExpression)expression).Method == typeof(string).GetMethod("Contains", new[] { typeof(string) }))
                {
                    return "(" + BuildWhereClause(((System.Linq.Expressions.MethodCallExpression)expression).Object) + " LIKE '%" + BuildWhereClause(((System.Linq.Expressions.MethodCallExpression)expression).Arguments[0], quote: false) + "%')";
                }

                if (((System.Linq.Expressions.MethodCallExpression)expression).Method == typeof(string).GetMethod("StartsWith", new[] { typeof(string) }))
                {
                    return "(" + BuildWhereClause(((System.Linq.Expressions.MethodCallExpression)expression).Object) + " LIKE '" + BuildWhereClause(((System.Linq.Expressions.MethodCallExpression)expression).Arguments[0], quote: false) + "%')";
                }

                if (((System.Linq.Expressions.MethodCallExpression)expression).Method == typeof(string).GetMethod("EndsWith", new[] { typeof(string) }))
                {
                    return "(" + BuildWhereClause(((System.Linq.Expressions.MethodCallExpression)expression).Object) + " LIKE '%" + BuildWhereClause(((System.Linq.Expressions.MethodCallExpression)expression).Arguments[0], quote: false) + "')";
                }

                if (((System.Linq.Expressions.MethodCallExpression)expression).Method.Name == "Contains")
                {
                    System.Linq.Expressions.Expression collection;
                    System.Linq.Expressions.Expression property;

                    if (((System.Linq.Expressions.MethodCallExpression)expression).Method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute)) && ((System.Linq.Expressions.MethodCallExpression)expression).Arguments.Count == 2)
                    {
                        collection = ((System.Linq.Expressions.MethodCallExpression)expression).Arguments[0];
                        property = ((System.Linq.Expressions.MethodCallExpression)expression).Arguments[1];
                    }
                    else if (!((System.Linq.Expressions.MethodCallExpression)expression).Method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute)) && ((System.Linq.Expressions.MethodCallExpression)expression).Arguments.Count == 1)
                    {
                        collection = ((System.Linq.Expressions.MethodCallExpression)expression).Object;
                        property = ((System.Linq.Expressions.MethodCallExpression)expression).Arguments[0];
                    }
                    else
                    {
                        throw new System.Exception("Unsupported expression");
                    }

                    System.Collections.IEnumerable values = (System.Collections.IEnumerable)GetValue(collection);
                    string concated = "";

                    foreach (var e in values)
                    {
                        concated += ValueToString(e, false, true) + ", ";
                    }

                    return concated == ""
                        ? ValueToString(false, true, false)
                        : "(" + BuildWhereClause(property) + " IN (" + concated.Substring(0, concated.Length - 2) + "))";
                }
            }
            throw new System.Exception("Unsupported expression");
        }

        private string GetColumnNameFor(string name)
        {
            return _columnMapping.ContainsKey(name) ? _columnMapping[name] : string.Empty;
        }

        private static string ValueToString(object value, bool isUnary, bool quote)
        {
            if (value is bool)
            {
                if (isUnary)
                {
                    return (bool)value ? "(1=1)" : "(1=0)";
                }

                return (bool)value ? "1" : "0";
            }

            if (quote)
                return "'" + value.ToString().Replace("'", "''") + "'";

            return value.ToString();
        }

        private static object GetValue(System.Linq.Expressions.Expression member)
        {
            // source: http://stackoverflow.com/a/2616980/291955
            var objectMember = System.Linq.Expressions.Expression.Convert(member, typeof(object));
            var getterLambda = System.Linq.Expressions.Expression.Lambda<System.Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static string NodeTypeToString(System.Linq.Expressions.ExpressionType nodeType, bool rightIsNull)
        {
            switch (nodeType)
            {
                case System.Linq.Expressions.ExpressionType.Add:
                    return "+";

                case System.Linq.Expressions.ExpressionType.And:
                    return "&";

                case System.Linq.Expressions.ExpressionType.AndAlso:
                    return "AND";

                case System.Linq.Expressions.ExpressionType.Divide:
                    return "/";

                case System.Linq.Expressions.ExpressionType.Equal:
                    return rightIsNull ? "IS" : "=";

                case System.Linq.Expressions.ExpressionType.GreaterThan:
                    return ">";

                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case System.Linq.Expressions.ExpressionType.LessThan:
                    return "<";

                case System.Linq.Expressions.ExpressionType.LessThanOrEqual:
                    return "<=";

                case System.Linq.Expressions.ExpressionType.Modulo:
                    return "%";

                case System.Linq.Expressions.ExpressionType.Multiply:
                    return "*";

                case System.Linq.Expressions.ExpressionType.Negate:
                    return "-";

                case System.Linq.Expressions.ExpressionType.Not:
                    return "NOT";

                case System.Linq.Expressions.ExpressionType.NotEqual:
                    return "<>";

                case System.Linq.Expressions.ExpressionType.Or:
                    return "|";

                case System.Linq.Expressions.ExpressionType.OrElse:
                    return "OR";

                case System.Linq.Expressions.ExpressionType.Subtract:
                    return "-";

                default:
                    throw new System.Exception(string.Format("Unsupported node type: {0}", nodeType));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
