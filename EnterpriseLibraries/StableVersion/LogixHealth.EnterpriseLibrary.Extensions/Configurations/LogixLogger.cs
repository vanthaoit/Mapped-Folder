namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    public class LogixLogger : ConfigurationElement
    {
        [ConfigurationProperty("ApplicationName", DefaultValue = "LogixApplication", IsRequired = true)]
        public string ApplicationName
        {
            get
            {
                return (string)this["ApplicationName"]; 
            }
            set
            {
                this["ApplicationName"] = value;
            }
        }

        [ConfigurationProperty("ServiceAddress", DefaultValue = "https://dlogger.logixhealth.com/EnterpriseLogger/LogixLoggerService.svc", IsRequired = true)]
        public string ServiceAddress
        {
            get
            {
                return (string)this["ServiceAddress"]; 
            }
            set
            {
                this["ServiceAddress"] = value;
            }
        }

        [ConfigurationProperty("ServiceBinding", DefaultValue = "wsHttpBinding", IsRequired = true)]
        public string ServiceBinding
        {
            get
            {
                return (string)this["ServiceBinding"]; 
            }
            set
            {
                this["ServiceBinding"] = value;
            }
        }

        [ConfigurationProperty("BaseLocationOfLogFile", DefaultValue = "D:\\LogixLogFiles\\", IsRequired = true)]
        public string BaseLocationOfLogFile
        {
            get
            {
                return (string)this["BaseLocationOfLogFile"]; 
            }
            set
            {
                this["BaseLocationOfLogFile"] = value;
            }
        }

        [ConfigurationProperty("UseMessageQueue", DefaultValue = "NO", IsRequired = true)]
        public string UseMessageQueue
        {
            get
            {
                return (string)this["UseMessageQueue"]; 
            }
            set
            {
                this["UseMessageQueue"] = value;
            }
        }

        [ConfigurationProperty("QueueLocation", DefaultValue = ".\\private$\\exceptionsLogs", IsRequired = true)]
        public string QueueLocation
        {
            get
            {
                return (string)this["QueueLocation"]; 
            }
            set
            {
                this["QueueLocation"] = value;
            }
        }
    }
}

