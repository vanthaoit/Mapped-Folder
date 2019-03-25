namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    public class LogixConfiguration : ConfigurationSection, ILogixConfiguration
    {
        public static ILogixConfiguration Configure()
        {
            return ConfigurationManager.GetSection("LogixConfigurations") as ILogixConfiguration;
        }

        private LogixConfiguration() { }

        [ConfigurationProperty(name: "ConnectAuthentication", IsRequired = false)]
        ConnectAuthentication ILogixConfiguration.ConnectAuthenticationSettings
        {
            get
            {
                return (ConnectAuthentication)this["ConnectAuthentication"]; 
            }
            set
            {
                value = (ConnectAuthentication)this["ConnectAuthentication"];
            }
        }

        [ConfigurationProperty(name: "HeaderAndFooter", IsRequired = false)]
        HeaderAndFooter ILogixConfiguration.HeaderAndFooterSettings
        {
            get
            {
                return (HeaderAndFooter)this["HeaderAndFooter"]; 
            }
            set
            {
                value = (HeaderAndFooter)this["HeaderAndFooter"];
            }
        }

        [ConfigurationProperty(name: "LogixLogger", IsRequired = true)]
        LogixLogger ILogixConfiguration.LogixLoggerSettings
        {
            get
            {
                return (LogixLogger)this["LogixLogger"]; 
            }
            set
            {
                value = (LogixLogger)this["LogixLogger"];
            }
        }

        [ConfigurationProperty(name: "ServiceEndpoints", IsRequired = false)]
        ServiceEndpointCollection ILogixConfiguration.ServiceEndpoints
        {
            get
            {
                return (ServiceEndpointCollection)this["ServiceEndpoints"];
            }
        }

        [ConfigurationProperty(name: "ApiEndpoints", IsRequired = false)]
        ApiEndpointCollection ILogixConfiguration.ApiEndpoints
        {
            get
            {
                return (ApiEndpointCollection)this["ApiEndpoints"];
            }
        }
    }
}

