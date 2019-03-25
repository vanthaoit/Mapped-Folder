using System;

namespace LogixHealth.EnterpriseLibrary.AppServices.Gateway
{
    internal enum GatewayBindings
    {
        basicHttpBinding,
        basicHttpsBinding,
        wsHttpBinding
    }

    public abstract class LogixServiceGateway<TService> where TService : class
    {
        private readonly string binding = string.Empty;

        private readonly string serviceEndpoint = string.Empty;

        protected LogixServiceGateway(string binding, string serviceEndpoint)
        {
            this.binding = binding;
            this.serviceEndpoint = serviceEndpoint;
        }

        public TService Gateway
        {
            get
            {
                System.ServiceModel.Channels.Binding endpointBinding = null;
                System.ServiceModel.EndpointAddress endpointAddress = null;

                if (this.binding.ToUpper() == GatewayBindings.basicHttpBinding.ToString().ToUpper())
                {
                    endpointBinding = CreateBindingForEndpoint(GatewayBindings.basicHttpBinding);
                    endpointAddress = CreateEndpointAddress(GatewayBindings.basicHttpBinding);
                }
                else if (this.binding.ToUpper() == GatewayBindings.basicHttpsBinding.ToString().ToUpper())
                {
                    endpointBinding = CreateBindingForEndpoint(GatewayBindings.basicHttpsBinding);
                    endpointAddress = CreateEndpointAddress(GatewayBindings.basicHttpsBinding);
                }
                else if (this.binding.ToUpper() == GatewayBindings.wsHttpBinding.ToString().ToUpper())
                {
                    endpointBinding = CreateBindingForEndpoint(GatewayBindings.wsHttpBinding);
                    endpointAddress = CreateEndpointAddress(GatewayBindings.wsHttpBinding);
                }

                System.ServiceModel.Channels.IChannelFactory<TService> channelFactory = new System.ServiceModel.ChannelFactory<TService>(endpointBinding, endpointAddress);
                return channelFactory.CreateChannel(endpointAddress);
            }
        }

        private System.ServiceModel.Channels.Binding CreateBindingForEndpoint(GatewayBindings binding)
        {
            if (binding == GatewayBindings.basicHttpsBinding)
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding
                {
                    MaxBufferSize = int.MaxValue,
                    ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                    MaxReceivedMessageSize = int.MaxValue,
                    AllowCookies = true
                };
                return result;
            }

            if (binding == GatewayBindings.wsHttpBinding)
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                result.Elements.Add(textBindingElement);

                System.ServiceModel.Channels.HttpsTransportBindingElement httpsBindingElement = new System.ServiceModel.Channels.HttpsTransportBindingElement
                {
                    AllowCookies = true,
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue
                };
                result.Elements.Add(httpsBindingElement);

                return result;
            }

            throw new InvalidOperationException(string.Format("Could not find endpoint configurations - \'{0}\'.", binding.ToString()));
        }

        private System.ServiceModel.EndpointAddress CreateEndpointAddress(GatewayBindings binding)
        {
            if (binding == GatewayBindings.basicHttpsBinding)
            {
                return new System.ServiceModel.EndpointAddress(serviceEndpoint);
            }

            if (binding == GatewayBindings.wsHttpBinding)
            {
                return new System.ServiceModel.EndpointAddress(serviceEndpoint);
            }

            throw new InvalidOperationException(string.Format("Could not find endpoint configurations - \'{0}\'.", binding.ToString()));
        }
    }
}
