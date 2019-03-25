namespace LogixHealth.EnterpriseLogger.Extensions
{
    using LogixHealth.EnterpriseLibrary.AppServices.Gateway;
    using LogixHealth.EnterpriseLogger.Services.DataContracts;
    using LogixHealth.EnterpriseLogger.Services.ServiceContracts;

    internal sealed class DatabaseLogger : LogixServiceGateway<ILogixLoggerService>
    {
        public DatabaseLogger(string binding, string serviceEndpoint) : base(binding, serviceEndpoint)
        {
        }

        public void LogExceptions(LogixLogException payLoad)
        {
            Gateway.LogExceptions(payLoad);
        }

        public void LogChangeDataCapture(LogixLogChangeDataCapture payLoad)
        {
            Gateway.LogChageDataCapture(payLoad);
        }

        public void LogUserEvents(LogixLogEvent payLoad)
        {
            Gateway.LogEvents(payLoad);
        }
    }
}