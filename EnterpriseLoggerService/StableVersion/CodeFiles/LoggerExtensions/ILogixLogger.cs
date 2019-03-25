namespace LogixHealth.EnterpriseLogger.Extensions
{
    public interface ILogixLogger : ILogixExceptionLogger, ILogixChangeDataCaptureLogger, ILogixUserEventLogger
    {
        Configuration Configuration { get; set; }
    }
}
