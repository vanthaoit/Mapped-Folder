namespace LogixHealth.EnterpriseLogger.Extensions
{
    using LogixHealth.EnterpriseLibrary.AppServices.Gateway;
    using LogixHealth.EnterpriseLogger.Services.DataContracts;

    internal class LoggerExtension : LogixServiceGateway<Services.ServiceContracts.ILogixLoggerService>
    {
        public LoggerExtension(string binding, string serviceEndpoint) : base(binding, serviceEndpoint)
        {
        }

        public void LogChageDataCapture(LogixLogChangeDataCapture payLoad)
        {
            base.Gateway.LogChageDataCapture(payLoad);
        }

        public void LogEvents(LogixLogEvent payLoad)
        {
            base.Gateway.LogEvents(payLoad);
        }

        public void LogExceptions(LogixLogException payLoad)
        {
            base.Gateway.LogExceptions(payLoad);
        }

        private static string ExtendException(dynamic exception)
        {
            string stackTrace = string.Empty;

            if (exception != null)
            {
                dynamic innerException = exception;
                while (innerException != null)
                {
                    stackTrace += innerException.Message + System.Environment.NewLine + innerException.StackTrace;
                    innerException = innerException.InnerException;
                }
            }

            string queueError = System.Environment.NewLine + " - Fault Exception Occurred While Calling Enterprise Logger Service -" + System.Environment.NewLine;

            if (stackTrace.Length > 0)
                queueError += stackTrace;
            else
                queueError = stackTrace;

            return queueError;
        }
    }
}
