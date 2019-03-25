namespace LogixHealth.EnterpriseLogger.Extensions
{
    public sealed class Configuration
    {
        public string ServiceAddress { get; set; }

        public string ServiceBinding { get; set; }

        public string BaseLocationOfLogFile { get; set; }

        public string UseMessageQueue { get; set; }

        public string QueueLocation { get; set; }

        public string ApplicationName { get; set; }
    }
}
