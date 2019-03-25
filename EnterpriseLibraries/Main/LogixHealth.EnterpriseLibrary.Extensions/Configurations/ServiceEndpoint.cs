namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    public class ServiceEndpoint : ConfigurationElement
    {
        [ConfigurationProperty(name: "Name", DefaultValue = "UniqueAppUserServiceName", IsRequired = true, IsKey = true)]
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
    }
}

