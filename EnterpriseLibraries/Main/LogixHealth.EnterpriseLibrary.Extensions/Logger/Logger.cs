namespace LogixHealth.EnterpriseLibrary.Extensions
{
    using LogixHealth.EnterpriseLogger.Extensions;

    public static class Logger
    {
        public static ILogixExceptionLogger ExceptionLogger { get; set; }

        public static ILogixChangeDataCaptureLogger CDCLogger { get; set; }

        public static ILogixUserEventLogger UserEventLogger { get; set; }
    }
}
