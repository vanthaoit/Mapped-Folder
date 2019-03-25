namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    public class ConnectAuthentication : ConfigurationElement
    {
        private ConnectAuthentication() { }

        [ConfigurationProperty("TokenProvider", DefaultValue = "https://devlhconnect.logixhealth.com/ConnectAuthentication/", IsRequired = true)]
        public string TokenProvider
        {
            get
            {
                return (string)this["TokenProvider"]; 
            }
            set
            {
                this["TokenProvider"] = value;
            }
        }

        [ConfigurationProperty("RelayingParty", DefaultValue = "https://devlhconnect.logixhealth.com/ConnectPortal/", IsRequired = true)]
        public string RelayingParty
        {
            get
            {
                return (string)this["RelayingParty"]; 
            }
            set
            {
                this["RelayingParty"] = value;
            }
        }

        [ConfigurationProperty("Thumbprint", DefaultValue = "EF8C19690385A06D060BF2C1B9717F9C132C2AA9", IsRequired = true)]
        public string Thumbprint
        {
            get
            {
                return (string)this["Thumbprint"]; 
            }
            set
            {
                this["Thumbprint"] = value;
            }
        }
    }
}

