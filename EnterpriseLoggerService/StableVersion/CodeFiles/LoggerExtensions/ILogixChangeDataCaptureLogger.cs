namespace LogixHealth.EnterpriseLogger.Extensions
{
    public interface ILogixChangeDataCaptureLogger
    {
        void LogChangeDataCapture(Services.DataContracts.LogixLogChangeDataCapture payLoad);
    }
}
