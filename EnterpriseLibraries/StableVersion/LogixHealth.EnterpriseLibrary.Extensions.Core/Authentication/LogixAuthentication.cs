namespace LogixHealth.EnterpriseLibrary.Extensions.Authentication
{
    using Microsoft.Extensions.DependencyInjection;

    using System.Linq;

    public static class ClaimTypes
    {
        public const string UserId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/objectguid";
        public const string UserName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string DisplayName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/fullname";
        public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    }

    public static class LogixAuthentication
    {
        private static string certificateFindValue = string.Empty;
        private static string authority = string.Empty;
        private static string audienceUrl = string.Empty;

        public static System.Security.Claims.ClaimsPrincipal LogixUser { get; set; }

        public static void AddLogixAuthentication(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            double sessionLifeTime = System.Convert.ToDouble(configuration["ConnectAuthentication:SessionLifeTime"]);
            string tokenValidatorServiceEndpoint = configuration["ConnectAuthentication:Extension:Address"];
            string tokenValidatorServiceBinding = configuration["ConnectAuthentication:Extension:Binding"];

            authority = configuration["ConnectAuthentication:Authority"];
            certificateFindValue = configuration["ConnectAuthentication:Thumbprint"];
            audienceUrl = configuration["ApplicationUrl"];

            Microsoft.IdentityModel.Tokens.X509SecurityKey securityKey = SecurityKey;

            // Creates Custom Token Validators
            System.Collections.ObjectModel.Collection<Microsoft.IdentityModel.Tokens.ISecurityTokenValidator> logixTokenValidators =
                new System.Collections.ObjectModel.Collection<Microsoft.IdentityModel.Tokens.ISecurityTokenValidator>
            {
                new LogixTokenValidator
                (
                    Microsoft.AspNetCore.Authentication.WsFederation.WsFederationDefaults.AuthenticationScheme,
                    securityKey.Certificate,
                    tokenValidatorServiceBinding,
                    tokenValidatorServiceEndpoint
                )
            };

            services
                .AddAuthentication
                (
                    options =>
                    {
                        options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.WsFederation.WsFederationDefaults.AuthenticationScheme;
                    }
                )
                .AddCookie()
                .AddWsFederation
                (
                    options =>
                    {
                        options.CallbackPath = "/";
                        options.Configuration = new Microsoft.IdentityModel.Protocols.WsFederation.WsFederationConfiguration
                        {
                            Issuer = authority,
                            TokenEndpoint = authority
                        };
                        options.SecurityTokenHandlers = logixTokenValidators;
                        options.SkipUnrecognizedRequests = true;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            IssuerSigningKey = securityKey,
                            TokenDecryptionKey = securityKey
                        };

                        options.Wreply = audienceUrl;
                        options.Wtrealm = audienceUrl;
                    }
                );
        }

        public static string Authority(this System.Security.Principal.IPrincipal principal)
        {
            return authority;
        }

        public static string AudienceUrl(this System.Security.Principal.IPrincipal principal)
        {
            return audienceUrl;
        }

        public static string ClaimsUserID(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.UserId);
        }

        private static string ClaimsUserEmail(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.Email);
        }

        private static string ClaimsUserName(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.UserName);
        }

        private static string ClaimsUserDisplayName(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.DisplayName);
        }

        private static Microsoft.IdentityModel.Tokens.X509SecurityKey SecurityKey
        {
            get
            {
                using (System.Security.Cryptography.X509Certificates.X509Store certificateStore =
                    new System.Security.Cryptography.X509Certificates.X509Store
                    (
                        System.Security.Cryptography.X509Certificates.StoreName.My,
                        System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine,
                        System.Security.Cryptography.X509Certificates.OpenFlags.OpenExistingOnly
                    ))
                {
                    System.Security.Cryptography.X509Certificates.X509Certificate2 certificate = null;
                    Microsoft.IdentityModel.Tokens.X509SecurityKey securityKey = null;
                    if (certificateStore != null)
                    {
                        System.Security.Cryptography.X509Certificates.X509Certificate2Collection certificates =
                            certificateStore.Certificates.Find
                            (
                                System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, certificateFindValue, true
                            );

                        if (certificates.Count > 0)
                        {
                            certificate = certificates[0];
                            securityKey = new Microsoft.IdentityModel.Tokens.X509SecurityKey(new System.Security.Cryptography.X509Certificates.X509Certificate2(certificate));
                        }
                    }

                    return securityKey;
                }
            }
        }

        private static string GetClaimValueByType(string claimType)
        {
            string result = string.Empty;

            if (LogixUser != null)
            {
                if (LogixUser.Identity is System.Security.Claims.ClaimsIdentity identity && identity.Claims.Count() > 1)
                {
                    System.Security.Claims.Claim claim = identity.Claims.FirstOrDefault(context => context.Type == claimType);
                    if (claim != null)
                        result = claim.Value;
                }
            }

            return result;
        }
    }
}
