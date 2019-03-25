namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    public class HeaderAndFooter : ConfigurationElement
    {
        [ConfigurationProperty("ServiceAddress", DefaultValue = "https://devlhconnect.logixhealth.com/ConnectPortalServices/AppUser.svc", IsRequired = true)]
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

        [ConfigurationProperty("UseNewConnectHeader", DefaultValue = "NO", IsRequired = true)]
        public string UseNewConnectHeader
        {
            get
            {
                return (string)this["UseNewConnectHeader"]; 
            }
            set
            {
                this["UseNewConnectHeader"] = value;
            }
        }
    }
}

