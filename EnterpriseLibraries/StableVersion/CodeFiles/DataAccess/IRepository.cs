namespace LogixHealth.EnterpriseLibrary.DataAccess
{
    /// <summary>
    /// Unit of Work is the concept related to the effective implementation of the repository pattern. 
    /// Non-generic repository pattern, Generic repository pattern. 
    /// Unit of Work is referred to as a single transaction that involves multiple operations of read/write
    /// </summary>
    public interface IUnitOfWork //: System.IDisposable
    {
        IRepository<T> CreateRepository<T>(string schema, string table) where T : class;

        //void CommitChanges();
    }

    /// <summary>
    /// Repository pattern, the domain entities, the data access logic and the business logic talk to each other using interfaces. 
    /// It hides the details of data access from the business logic.
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> : System.IDisposable where T : class
    {
        /// <summary>
        /// This method fetches all the rows from the respective Table
        /// </summary>
        /// <returns>collection of entities</returns>
        System.Collections.Generic.ICollection<T> AllRecords();

        /// <summary>
        /// This method fetches all the rows from the respective Table based on the where clause
        /// </summary>
        /// <param name="where">filters</param>
        /// <returns>collection of entities</returns>
        System.Collections.Generic.ICollection<T> FindRecords(System.Linq.Expressions.Expression<System.Func<T, bool>> where);

        /// <summary>
        /// This method used to get a specific row from the respective Table based on the where clause
        /// </summary>
        /// <param name="where">filters</param>
        /// <returns>object of type T</returns>
        T SingleRecord(System.Linq.Expressions.Expression<System.Func<T, bool>> where);
    }
}

