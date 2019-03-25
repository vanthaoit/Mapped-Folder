namespace LogixHealth.EnterpriseLibrary.Extensions
{
    using System.Linq;

    public static class ConfigurationManager
    {
        private static Configurations.ILogixConfiguration _configuration = null;

        public static System.Security.Claims.ClaimsPrincipal LoggedInUser { get; set; }

        public static Configurations.ILogixConfiguration InitializeConfigurations()
        {
            _configuration = Configurations.LogixConfiguration.Configure();

            return _configuration;
        }

        public static void InitializeExceptionLogger()
        {
            Logger.ExceptionLogger = EnterpriseLogger.Extensions.LoggerStartUp.ConfigureExceptionLogger
                (
                    new EnterpriseLogger.Extensions.Configuration
                    {
                        ApplicationName = _configuration.LogixLoggerSettings.ApplicationName,
                        BaseLocationOfLogFile = _configuration.LogixLoggerSettings.BaseLocationOfLogFile,
                        QueueLocation = _configuration.LogixLoggerSettings.QueueLocation,
                        ServiceAddress = _configuration.LogixLoggerSettings.ServiceAddress,
                        ServiceBinding = _configuration.LogixLoggerSettings.ServiceBinding,
                        UseMessageQueue = _configuration.LogixLoggerSettings.UseMessageQueue
                    }
                );
        }

        public static void InitializeChangeDataCaptureLogger()
        {
            Logger.CDCLogger = EnterpriseLogger.Extensions.LoggerStartUp.ConfigureChangeDataCaptureLogger
                (
                    new EnterpriseLogger.Extensions.Configuration
                    {
                        ApplicationName = _configuration.LogixLoggerSettings.ApplicationName,
                        BaseLocationOfLogFile = _configuration.LogixLoggerSettings.BaseLocationOfLogFile,
                        QueueLocation = _configuration.LogixLoggerSettings.QueueLocation,
                        ServiceAddress = _configuration.LogixLoggerSettings.ServiceAddress,
                        ServiceBinding = _configuration.LogixLoggerSettings.ServiceBinding,
                        UseMessageQueue = _configuration.LogixLoggerSettings.UseMessageQueue
                    }
                );
        }

        public static void InitializeUserEventLogger()
        {
            Logger.UserEventLogger = EnterpriseLogger.Extensions.LoggerStartUp.ConfigureUserEventLogger
                (
                    new EnterpriseLogger.Extensions.Configuration
                    {
                        ApplicationName = _configuration.LogixLoggerSettings.ApplicationName,
                        BaseLocationOfLogFile = _configuration.LogixLoggerSettings.BaseLocationOfLogFile,
                        QueueLocation = _configuration.LogixLoggerSettings.QueueLocation,
                        ServiceAddress = _configuration.LogixLoggerSettings.ServiceAddress,
                        ServiceBinding = _configuration.LogixLoggerSettings.ServiceBinding,
                        UseMessageQueue = _configuration.LogixLoggerSettings.UseMessageQueue
                    }
                );
        }

        public static void InitializeHeaderAndFooter()
        {
            if (LoggedInUser == null) return;

            if (LoggedInUser.Claims.Count() == 0) return;

            if (!LoggedInUser.Identity.IsAuthenticated) return;

            string organizationId = "0";
            HeadersAndFooters.LogixHeaderAndFooterServiceClient serviceClient = new HeadersAndFooters.LogixHeaderAndFooterServiceClient
                                                                                    (
                                                                                        _configuration.HeaderAndFooterSettings.ServiceBinding,
                                                                                        _configuration.HeaderAndFooterSettings.ServiceAddress
                                                                                    );
            HeadersAndFooters.LogixPortal.UseNewHeaderFooter = null;

            if (_configuration.HeaderAndFooterSettings.UseNewConnectHeader.ToUpper() == "YES")
            {
                string helpId = string.Empty;
                string timeOut = string.Empty;

                HeadersAndFooters.LogixPortal.Header = serviceClient.GetUserHeaderForCurrentArea
                                    (
                                        new System.Guid(LoggedInUser.UserID()),
                                        _configuration.LogixLoggerSettings.ApplicationName,
                                        helpId,
                                        timeOut,
                                        organizationId
                                    );

                HeadersAndFooters.LogixPortal.Footer = serviceClient.GetUserFooterCurrentArea
                                    (
                                        _configuration.LogixLoggerSettings.ApplicationName
                                    );
                if (HeadersAndFooters.LogixPortal.Header != null)
                    HeadersAndFooters.LogixPortal.UseNewHeaderFooter = true;
            }
            else
            {
                organizationId = "";
                HeadersAndFooters.LogixPortal.Header = serviceClient.GetUserHeader
                                    (
                                        new System.Guid(LoggedInUser.UserID()),
                                        organizationId
                                    );
                if (HeadersAndFooters.LogixPortal.Header != null)
                {
                    string htmlHeader = "" +
                        "<div class=\"page - header\"> " +
                        "   <div class=\"nav - bar\">" +
                        "       <div class=\"container cf\">" +
                        "           <a href=\"#\"><i class=\"icn logo paidright\"></i></a>" +
                        "           <ul class=\"nav\">" +
                        "               <li class=\"level-1-first\" style=\"z-index: 1\">" +
                        "                   <a class=\"user\" tabindex=\"0\" id=\"userName\">" +
                        "                       <span id=\"spnGreetings\"></span>" +
                        "                   </a>" +
                        "                   <div class=\"level-2\" style=\"height: 32px;\">" +
                        "                       <dl class=\"app-alerts user-menu\">" +
                        "                           <dt>" +
                        "                               <input type=\"hidden\" id=\"hdnLoggedUserId\" />" +
                        "                               <ul class=\"main\">" +
                        "                                   <li><a href=\"~/Home/Logout\"><i class=\"icn logout\"></i></a></li>" +
                        "                                   <li style=\"margin-top: 5px;\"><h6><a class=\"clr1-text\" href=\"~/Home/Logout\">Logout</a></h6></li>" +
                        "                                   @Html.Hidden(\"ApplicationPath\", Url.Content(\"~/\"))" +
                        "                               </ul>" +
                        "                           </dt>" +
                        "                       </dl>" +
                        "                   </div>" +
                        "               </li>" +
                        "           </ul>" +
                        "       </div>" +
                        "   </div>" +
                        "</div>";

                    HeadersAndFooters.LogixPortal.Header.ApplicationName = _configuration.LogixLoggerSettings.ApplicationName;
                    HeadersAndFooters.LogixPortal.Header.OutputHtmlHeader = htmlHeader;
                }
                HeadersAndFooters.LogixPortal.UseNewHeaderFooter = false;
            }
        }

        public static void UseLogixAuthentication()
        {
            //
        }
    }

    public static class ClaimsExtensions
    {
        public static string UserID(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.UserId);
        }

        public static string UserEmail(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.Email);
        }

        public static string UserName(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.UserName);
        }

        public static string UserDisplayName(this System.Security.Principal.IPrincipal principal)
        {
            return GetClaimValueByType(ClaimTypes.DisplayName);
        }

        private static string GetClaimValueByType(string claimType)
        {
            string result = string.Empty;

            if (ConfigurationManager.LoggedInUser != null)
            {
                if (ConfigurationManager.LoggedInUser.Identity is System.Security.Claims.ClaimsIdentity)
                {
                    System.Security.Claims.ClaimsIdentity identity = (System.Security.Claims.ClaimsIdentity)ConfigurationManager.LoggedInUser.Identity;

                    if (identity.Claims.Count() > 1)
                    {
                        System.Security.Claims.Claim claim = identity.Claims.FirstOrDefault(context => context.Type == claimType);

                        if (claim != null)
                        {
                            result = claim.Value;
                        }
                    }
                }
            }

            return result;
        }
    }

    internal static class ClaimTypes
    {
        public const string UserId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/objectguid";
        public const string UserName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string DisplayName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/fullname";
        public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    }
}

