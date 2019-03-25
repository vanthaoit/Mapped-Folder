using Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LogixHealth.EnterpriseLibrary.DataAccess
{
    public class QueryRepository<T> : QueryRepository, IQueryRepository<T> where T : new()
    {
        private readonly string _connectionString;
        private readonly int? _commandTimeOut = 20;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing == true)
        //    {
        //        //Let us release all the managed resources
        //        if (Connection != null)
        //            if (Connection.State != ConnectionState.Closed)
        //            {
        //                Connection.Close();
        //                Connection.Dispose();
        //            }
        //    }
        //}

        //public void Dispose()
        //{
        //    // If this function is being called the user wants to release the
        //    // resources. lets call the Dispose which will do this for us.
        //    Dispose(true);

        //    // Now since we have done the cleanup already there is nothing left
        //    // for the Finalizer to do. So lets tell the GC not to call it later.
        //    GC.SuppressFinalize(this);
        //}

        //internal IDbConnection Connection
        //{
        //    get
        //    {
        //        return new SqlConnection(_connectionString);
        //    }
        //}

        public QueryRepository(string connectionString, int? timeOut = 20) : base(connectionString, timeOut)
        {
            _connectionString = connectionString;
            _commandTimeOut = timeOut;
        }

        public IEnumerable<T> All(string storedProcedureName, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    return SqlMapper.Query<T>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        ).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<T> GetData(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, parameter.Value);

                    return SqlMapper.Query<T>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        ).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<T> GetData(string storedProcedureName, IDictionary<string, object> parameters, string tableTypeName, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, ToDataTable(parameter.Value, tableTypeName).AsTableValuedParameter(tableTypeName));

                    return SqlMapper.Query<T>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        ).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T SingleOrDefault(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, parameter.Value);

                    var result = SqlMapper.Query<T>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                    if (result != null)
                    {
                        return result.FirstOrDefault();
                    }

                    return default(T);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T SingleOrDefault(string storedProcedureName, IDictionary<string, object> parameters, string tableTypeName, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, ToDataTable(parameter.Value, tableTypeName).AsTableValuedParameter(tableTypeName));

                    var result = SqlMapper.Query<T>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                    if (result != null)
                    {
                        return result.FirstOrDefault();
                    }

                    return default(T);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetScalarValue(string storedProcedureName, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    return SqlMapper.ExecuteScalar<string>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetScalarValue(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, parameter.Value);

                    return SqlMapper.ExecuteScalar<string>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertOrUpdate(string storedProcedureName, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    SqlMapper.Execute
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertOrUpdate(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, parameter.Value);

                    SqlMapper.Execute
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertOrUpdate(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();

                    foreach (var parameter in parameters)
                        sqlParameters.Add
                            (
                                "@" + parameter.Name,
                                (parameter.IsTableType) ? ToDataTable(parameter.Value, parameter.TableTypeName).AsTableValuedParameter(parameter.TableTypeName) : parameter.Value
                            );

                    SqlMapper.Execute
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IDictionary<string, object> InsertOrUpdate(string storedProcedureName, IDictionary<string, Tuple<object, ParameterDirection>> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, parameter.Value.Item1, direction: (parameter.Value.Item2 == ParameterDirection.InputOutput || parameter.Value.Item2 == ParameterDirection.Output) ? ParameterDirection.InputOutput : ParameterDirection.Input);

                    SqlMapper.Execute
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                    IDictionary<string, object> dictionary = new Dictionary<string, object>();
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Value.Item2 == ParameterDirection.InputOutput || parameter.Value.Item2 == ParameterDirection.Output)
                            dictionary.Add(parameter.Key, sqlParameters.Get<object>(parameter.Key));
                    }

                    return dictionary;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool BulkInsertOrUpdate(string storedProcedureName, IDictionary<string, object> parameters, string tableTypeName, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, ToDataTable(parameter.Value, tableTypeName).AsTableValuedParameter(tableTypeName));

                    SqlMapper.Execute
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add("@" + parameter.Key, parameter.Value);

                    SqlMapper.Execute
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                            commandTimeout: _commandTimeOut
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class QueryRepository : IQueryRepository
    {
        private readonly string _connectionString;
        private readonly int? _commandTimeOut = 20;

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public QueryRepository(string connectionString, int? timeOut = 20)
        {
            _connectionString = connectionString;
            _commandTimeOut = timeOut;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //Let us release all the managed resources
                if (Connection != null)
                    if (Connection.State != ConnectionState.Closed)
                    {
                        Connection.Close();
                        Connection.Dispose();
                    }
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

        IEnumerable<TResult> IQueryRepository.GetData<TResult>(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add
                            (
                                "@" + parameter.Name,
                                (parameter.IsTableType) ? ToDataTable(parameter.Value, parameter.TableTypeName).AsTableValuedParameter(parameter.TableTypeName) : parameter.Value
                            );

                    return SqlMapper.Query<TResult>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure,
                            commandTimeout: _commandTimeOut
                        ).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        TResult IQueryRepository.GetScalarValue<TResult>(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                        sqlParameters.Add
                            (
                                "@" + parameter.Name,
                                parameter.IsTableType ? ToDataTable(parameter.Value, parameter.TableTypeName).AsTableValuedParameter(parameter.TableTypeName) : parameter.Value
                            );

                    return SqlMapper.ExecuteScalar<TResult>
                        (
                            cnn: connection,
                            sql: storedProcedureName,
                            param: sqlParameters,
                            commandType: commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure,
                            commandTimeout: _commandTimeOut
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        IEnumerable<TResult> IQueryRepository.InsertOrUpdate<TResult>(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType)
        {
            try
            {
                using (IDbConnection connection = Connection)
                {
                    DynamicParameters sqlParameters = new DynamicParameters();

                    foreach (var parameter in parameters)
                        sqlParameters.Add
                            (
                                "@" + parameter.Name,
                                parameter.IsTableType
                                    ? ToDataTable(parameter.Value, parameter.TableTypeName).AsTableValuedParameter(parameter.TableTypeName) 
                                    : parameter.Value
                            );

                    return SqlMapper.Query<TResult>
                            (
                                cnn: connection,
                                sql: storedProcedureName,
                                param: sqlParameters,
                                commandType: (commandType == CommandType.InlineQuery ? System.Data.CommandType.Text : System.Data.CommandType.StoredProcedure),
                                commandTimeout: _commandTimeOut
                            );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Converts a List of object to a DataTable.
        /// </summary>
        /// <typeparam name="T">The type of the list collection.</typeparam>
        /// <param name="objectCollection">List instance reference.</param>
        /// <returns>A DataTable of the converted list collection.</returns>
        internal static DataTable ToDataTable(dynamic objectCollection, string tableName)
        {
            DataTable table = new DataTable(tableName);

            System.Reflection.PropertyInfo[] columns = null;

            if (objectCollection == null) return table;

            Type type = objectCollection.GetType();
            dynamic collection = new List<dynamic>();

            if (type.IsGenericType || type.IsArray)
                collection = objectCollection;
            else
                collection.Add(objectCollection);

            foreach (var record in collection)
            {
                if (columns == null)
                {
                    columns = record.GetType().GetProperties();
                    foreach (System.Reflection.PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if (colType.IsGenericType && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            colType = colType.GetGenericArguments()[0];

                        if (GetProperty.CustomAttributes.Count() > 0)
                            if (GetProperty.CustomAttributes
                                    .Where
                                        (
                                            ca =>
                                                ca.NamedArguments.Count() > 0 &&
                                                ca.NamedArguments.Where
                                                (
                                                    na =>
                                                        na.MemberName == "Order" &&
                                                        (int)na.TypedValue.Value == 9999
                                                ).Count() > 0
                                        ).Count() > 0)
                                continue;

                        table.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = table.NewRow();

                foreach (System.Reflection.PropertyInfo pinfo in columns)
                {
                    if (pinfo.CustomAttributes.Count() > 0)
                        if (pinfo.CustomAttributes
                                .Where
                                    (
                                        ca =>
                                            ca.NamedArguments.Count() > 0 &&
                                            ca.NamedArguments.Where
                                            (
                                                na =>
                                                    na.MemberName == "Order" &&
                                                    (int)na.TypedValue.Value == 9999
                                            ).Count() > 0
                                    ).Count() > 0)
                            continue;

                    dr[pinfo.Name] = pinfo.GetValue(record) ?? DBNull.Value;
                }

                table.Rows.Add(dr);
            }

            return table;
        }
    }
}

