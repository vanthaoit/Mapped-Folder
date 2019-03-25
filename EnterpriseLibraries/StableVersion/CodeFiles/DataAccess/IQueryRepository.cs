namespace LogixHealth.EnterpriseLibrary.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public enum CommandType
    {
        InlineQuery,
        StoredProcedure
    }

    /// <summary>
    /// IQueryRepository is an interface for repositories
    /// </summary>
    /// <typeparam name="T">Entity/POCO object for which repository methods will be executed</typeparam>
    public interface IQueryRepository<T> : IQueryRepository where T : new()
    {
        /// <summary>
        /// Fetches all the records/data from database
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>List of objects</returns>
        IEnumerable<T> All(string storedProcedureName, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Fetches all the records/data from database based on the filter criteria (i.e. parameters)
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>List of objects</returns>
        IEnumerable<T> GetData(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Fetches all the records/data from database based on the filter criteria (i.e. parameters)
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="tableTypeName">name of the table type passed as parameter</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>List of objects</returns>
        IEnumerable<T> GetData(string storedProcedureName, IDictionary<string, object> parameters, string tableTypeName, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Finds the specific record from database based on the filter criteria
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>respective entity/object/poco</returns>
        T SingleOrDefault(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Finds the specific record from database based on the filter criteria
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="tableTypeName">name of the table type passed as parameter</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>respective entity/object/poco</returns>
        T SingleOrDefault(string storedProcedureName, IDictionary<string, object> parameters, string tableTypeName, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Finds the specific record from database based on the filter criteria
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>respective string</returns>
        string GetScalarValue(string storedProcedureName, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Finds the specific record from database based on the filter criteria
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>respective string</returns>
        string GetScalarValue(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Inserts/Updates the changes to the database object (i.e. table) based on the entity state
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>affected rows</returns>
        bool InsertOrUpdate(string storedProcedureName, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Inserts/Updates the changes to the database object (i.e. table) based on the entity state
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>affected rows</returns>
        bool InsertOrUpdate(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Inserts/Updates the changes to the database object and it`s child (i.e. master/detail table) based on the entity state
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair and direction of the parameter</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>returns values for the out parameters</returns>
        bool InsertOrUpdate(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Inserts/Updates the changes to the database object (i.e. table) based on the entity state
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair and direction of the parameter</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>returns values for the out parameters</returns>
        [Obsolete("This method is will be removed soon. Instead use InsertOrUpdate(string, IEnumerable<NameValueType>, CommandType) method.")]
        IDictionary<string, object> InsertOrUpdate(string storedProcedureName, IDictionary<string, Tuple<object, ParameterDirection>> parameters, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Inserts/Updates the changes to the database object (i.e. table) based on the entity state
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="tableTypeName">name of the table type passed as parameter</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>affected rows</returns>
        bool BulkInsertOrUpdate(string storedProcedureName, IDictionary<string, object> parameters, string tableTypeName, CommandType commandType = CommandType.StoredProcedure);

        /// <summary>
        /// Deletes the record in the database object(table)
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>affected rows</returns>
        void Delete(string storedProcedureName, IDictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);
    }

    public interface IQueryRepository : IDisposable
    {
        /// <summary>
        /// Fetches all the records/data from database based on the filter criteria (i.e. parameters)
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">name-value collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>List of objects</returns>
        IEnumerable<TResult> GetData<TResult>(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType = CommandType.StoredProcedure) where TResult : new();

        /// <summary>
        /// Finds the specific record from database based on the filter criteria
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>respective string</returns>
        TResult GetScalarValue<TResult>(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType = CommandType.StoredProcedure) where TResult : new();

        /// <summary>
        /// Inserts/Updates the changes to the database object and it`s child (i.e. master/detail table) based on the entity state
        /// </summary>
        /// <param name="storedProcedureName">inline query or sp name</param>
        /// <param name="parameters">dictionary collection with Key/Value (key - parameter name, value - value for that parameter) pair and direction of the parameter</param>
        /// <param name="commandType">command type - Text or Stored Procedure, default is SP</param>
        /// <returns>returns values for the out parameters</returns>
        IEnumerable<TResult> InsertOrUpdate<TResult>(string storedProcedureName, IEnumerable<NameValueType> parameters, CommandType commandType = CommandType.StoredProcedure) where TResult : new();
    }

    public class NameValueType
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public bool IsTableType { get; set; }

        public string TableTypeName { get; set; }

        public ParameterDirection Direction { get; set; }
    }
}