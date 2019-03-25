namespace LogixHealth.EnterpriseLogger.Extensions
{
    public interface ILogixExceptionLogger
    {
        void LogException(Services.DataContracts.LogixLogError payLoad);

        void LogException(Services.DataContracts.LogixLogError payLoad, dynamic httpContext);
    }
}
