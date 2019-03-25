namespace LogixHealth.EnterpriseLibrary.Extensions.Authentication
{
    [System.ServiceModel.ServiceContractAttribute]
    public interface IAuthenticationExtensionService
    {
        [System.ServiceModel.OperationContractAttribute]
        string Decrypt(string token);
    }

    public class LogixTokenValidatorServiceClient : AppServices.Gateway.LogixServiceGateway<IAuthenticationExtensionService>
    {
        public LogixTokenValidatorServiceClient(string binding, string serviceEndpoint) : base(binding, serviceEndpoint)
        {
        }

        public string Decrypt(string token)
        {
            return base.Gateway.Decrypt(token);
        }
    }
}
