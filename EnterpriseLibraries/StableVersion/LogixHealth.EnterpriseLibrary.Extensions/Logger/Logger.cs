namespace LogixHealth.EnterpriseLibrary.Extensions
{
    public static class Logger
    {
        public static EnterpriseLogger.Extensions.ILogixExceptionLogger ExceptionLogger { get; set; }

        public static EnterpriseLogger.Extensions.ILogixChangeDataCaptureLogger CDCLogger { get; set; }

        public static EnterpriseLogger.Extensions.ILogixUserEventLogger UserEventLogger { get; set; }
    }
}
