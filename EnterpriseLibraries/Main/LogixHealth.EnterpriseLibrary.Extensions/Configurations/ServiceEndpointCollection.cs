namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    [ConfigurationCollection(typeof(ServiceEndpoint), AddItemName = "ServiceEndpoint")]
    public class ServiceEndpointCollection : ConfigurationElementCollection
    {
        public new ServiceEndpoint this[string key]
        {
            get
            {
                return (ServiceEndpoint)BaseGet(key);
            }
            set
            {
                if (BaseGet(key) != null)
                {
                    BaseRemove(key);
                }

                BaseAdd(Count - 1, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceEndpoint();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceEndpoint)element).Name;
        }
    }
}

