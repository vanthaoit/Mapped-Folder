namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    public class ApiEndpoint : ConfigurationElement
    {
        [ConfigurationProperty(name: "Name", DefaultValue = "UniqueAPIName", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["Name"];
            }
            set
            {
                this["Name"] = value;
            }
        }

        [ConfigurationProperty("ApiAddress", DefaultValue = "https://dlogger.logixhealth.com/EnterpriseLogger/LogixLoggerService.svc", IsRequired = true)]
        public string ApiAddress
        {
            get
            {
                return (string)this["ApiAddress"];
            }
            set
            {
                this["ApiAddress"] = value;
            }
        }
    }
}

