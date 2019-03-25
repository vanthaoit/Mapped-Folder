using LogixHealth.EnterpriseLogger.Services.DataContracts;
using LogixHealth.EnterpriseLogger.Services.ServiceContracts;

namespace LogixHealth.EnterpriseLogger.Services
{
    public class LogixLoggerService : ServiceContracts.ILogixLoggerService
    {
        void ILogixLoggerService.LogChageDataCapture(LogixLogChangeDataCapture payLoad)
        {
            throw new System.NotImplementedException();
        }

        void ILogixLoggerService.LogEvents(LogixLogEvent payLoad)
        {
            throw new System.NotImplementedException();
        }

        void ILogixLoggerService.LogExceptions(LogixLogException payLoad)
        {
            try
            {
                MsmqHelper.WriteExceptionLogs(payLoad);
            }
            catch (System.Exception exception)
            {
                payLoad.UserErrorInfo.StackTrace += System.Environment.NewLine + ExtendException(exception);
                throw new System.ServiceModel.FaultException<LogixLogFaultException>
                    (
                        new LogixLogFaultException
                        {
                            FaultID = System.Guid.NewGuid(),
                            ErrorInfo = payLoad
                        },
                        exception.Message
                    );
            }
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

            string queueError = System.Environment.NewLine + " - Exception Occurred While Writting Logs -" + System.Environment.NewLine;

            if (stackTrace.Length > 0)
                queueError += stackTrace;
            else
                queueError = stackTrace;

            return queueError;
        }
    }
}
