namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    [ConfigurationCollection(typeof(ApiEndpoint), AddItemName = "ApiEndpoint")]
    public class ApiEndpointCollection : ConfigurationElementCollection
    {
        public new ApiEndpoint this[string key]
        {
            get
            {
                return (ApiEndpoint)BaseGet(key);
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
            return new ApiEndpoint();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ApiEndpoint)element).Name;
        }
    }
}

