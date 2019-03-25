namespace LogixHealth.EnterpriseLogger.Extensions
{
    public static class LoggerStartUp
    {
        public static ILogixLogger ConfigureExceptionLogger(Configuration configuration)
        {
            string date = System.DateTime.Today.Year.ToString() +
                            System.DateTime.Today.Month.ToString() +
                            System.DateTime.Today.Day.ToString();
            string fileName = string.Format("{0}_{1}_exceptionslogs.log", configuration.ApplicationName, date);
            string folderName = System.IO.Path.Combine(configuration.BaseLocationOfLogFile, configuration.ApplicationName);

            NLog.Targets.FileTarget fileTarget = new NLog.Targets.FileTarget
            {
                FileName = System.IO.Path.Combine(folderName, fileName),
                Layout = "${date:format=yyyy/MMM/dd HH\\:mm\\:ss.ffff} | ${level:uppercase=true:padding=6} | ${message}",
                ConcurrentWrites = true,
                CreateDirs = true
            };

            NLog.Config.LoggingRule rule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, fileTarget);
            NLog.Config.LoggingConfiguration nlogConfiguration = new NLog.Config.LoggingConfiguration();
            nlogConfiguration.LoggingRules.Add(rule);
            NLog.LogManager.Configuration = nlogConfiguration;
            NLog.Logger logger = NLog.LogManager.GetLogger("LogixLogger");

            return new LogixLogger(logger, configuration);
        }

        public static ILogixLogger ConfigureChangeDataCaptureLogger(Configuration configuration)
        {
            string date = System.DateTime.Today.Year.ToString() +
                            System.DateTime.Today.Month.ToString() +
                            System.DateTime.Today.Day.ToString();
            string fileName = string.Format("{0}_{1}_changedatacapturelogs.log", configuration.ApplicationName, date);

            NLog.Targets.FileTarget fileTarget = new NLog.Targets.FileTarget
            {
                FileName = System.IO.Path.Combine(configuration.BaseLocationOfLogFile, fileName),
                Layout = "${date:format=yyyy/MMM/dd HH\\:mm\\:ss.ffff} | ${level:uppercase=true:padding=6} | ${message}",
                ConcurrentWrites = true,
                CreateDirs = true
            };

            NLog.Config.LoggingRule rule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, fileTarget);
            NLog.Config.LoggingConfiguration nlogConfiguration = new NLog.Config.LoggingConfiguration();
            nlogConfiguration.LoggingRules.Add(rule);
            NLog.LogManager.Configuration = nlogConfiguration;
            NLog.Logger logger = NLog.LogManager.GetLogger("LogixLogger");

            return new LogixLogger(logger, configuration);
        }

        public static ILogixLogger ConfigureUserEventLogger(Configuration configuration)
        {
            string date = System.DateTime.Today.Year.ToString() +
                            System.DateTime.Today.Month.ToString() +
                            System.DateTime.Today.Day.ToString();
            string fileName = string.Format("{0}_{1}_usereventlogs.log", configuration.ApplicationName, date);

            NLog.Targets.FileTarget fileTarget = new NLog.Targets.FileTarget
            {
                FileName = System.IO.Path.Combine(configuration.BaseLocationOfLogFile, fileName),
                Layout = "${date:format=yyyy/MMM/dd HH\\:mm\\:ss.ffff} | ${level:uppercase=true:padding=6} | ${message}",
                ConcurrentWrites = true,
                CreateDirs = true
            };

            NLog.Config.LoggingRule rule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, fileTarget);
            NLog.Config.LoggingConfiguration nlogConfiguration = new NLog.Config.LoggingConfiguration();
            nlogConfiguration.LoggingRules.Add(rule);
            NLog.LogManager.Configuration = nlogConfiguration;
            NLog.Logger logger = NLog.LogManager.GetLogger("LogixLogger");

            return new LogixLogger(logger, configuration);
        }
    }
}
