namespace LogixHealth.EnterpriseLibrary.Extensions.Authentication
{
    public class LogixTokenValidator : Microsoft.IdentityModel.Tokens.ISecurityTokenValidator
    {
        private readonly string binding;

        private readonly string endpoint;

        private readonly string authenticationScheme;

        private readonly System.Security.Cryptography.X509Certificates.X509Certificate2 certificate;

        public LogixTokenValidator(string authenticationScheme, System.Security.Cryptography.X509Certificates.X509Certificate2 certificate, string binding, string endpoint)
        {
            this.authenticationScheme = authenticationScheme;
            this.certificate = certificate;
            this.binding = binding;
            this.endpoint = endpoint;
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get => Microsoft.IdentityModel.Tokens.TokenValidationParameters.DefaultMaximumTokenSizeInBytes; set => value = Microsoft.IdentityModel.Tokens.TokenValidationParameters.DefaultMaximumTokenSizeInBytes; }

        public bool CanReadToken(string securityToken) => true; //tokenHandler.CanReadToken(securityToken);

        public System.Security.Claims.ClaimsPrincipal ValidateToken(string securityToken, Microsoft.IdentityModel.Tokens.TokenValidationParameters validationParameters, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken)
        {
            validationParameters.ValidateIssuer = false;
            validatedToken = null;
            string decryptedSecurityToken = Decrypt(securityToken);

            LogixAuthentication.LogixUser = new Microsoft.IdentityModel.Tokens.Saml.SamlSecurityTokenHandler().ValidateToken(decryptedSecurityToken, validationParameters, out validatedToken);

            return LogixAuthentication.LogixUser;
        }

        private string Decrypt(string securityToken)
        {
            LogixTokenValidatorServiceClient serviceClient = new LogixTokenValidatorServiceClient(binding, endpoint);
            return serviceClient.Decrypt(securityToken);
        }
    }
}
