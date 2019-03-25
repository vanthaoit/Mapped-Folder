namespace LogixHealth.EnterpriseLogger.Extensions
{
    public interface ILogixUserEventLogger
    {
        void LogEvents(Services.DataContracts.LogixLogEvent payLoad);
    }
}
