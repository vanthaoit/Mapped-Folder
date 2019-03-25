namespace LogixHealth.EnterpriseLibrary.Extensions.HeadersAndFooters
{
    using LogixHealth.EnterpriseLibrary.Extensions.Authentication;
    using System.Linq;

    public class LogixConnectHeader
    {
        public Connect.BusinessEntities.UserHeaderInfo TheNewConnectHeader { get; set; }

        public Connect.BusinessEntities.UserHeader TheOldConnectHeader { get; set; }
    }

    public static class LogixHeader
    {
        public static LogixConnectHeader ConnectHeader { get; set; }

        public static void AddLogixAppHeaders(this Microsoft.AspNetCore.Builder.IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            if (LogixAuthentication.LogixUser == null) return;

            if (LogixAuthentication.LogixUser.Claims.Count() == 0) return;

            string applicationName = configuration["ApplicationName"];
            string useNewConnectHeader = configuration["HeaderAndFooter:UseNewConnectHeader"];
            string endpoint = configuration["HeaderAndFooter:Address"];
            string binding = configuration["HeaderAndFooter:Binding"];

            string helpId = string.Empty;
            string timeOut = string.Empty;
            string organizationId = "";

            LogixHeaderAndFooterServiceClient serviceClient = new LogixHeaderAndFooterServiceClient(binding, endpoint);
            ConnectHeader = new LogixConnectHeader();

            if (useNewConnectHeader.ToUpper() == "YES")
            {
                ConnectHeader.TheNewConnectHeader = serviceClient.GetUserHeaderForCurrentArea(new System.Guid(LogixAuthentication.LogixUser.ClaimsUserID()), applicationName, helpId, timeOut, organizationId);
                if (ConnectHeader.TheNewConnectHeader != null)
                {
                    ConnectHeader.TheNewConnectHeader.ApplicationName = applicationName;
                }
            }
            else if (useNewConnectHeader.ToUpper() == "NO")
            {
                ConnectHeader.TheOldConnectHeader = serviceClient.GetUserHeader(new System.Guid(LogixAuthentication.LogixUser.ClaimsUserID()), organizationId);
                if (ConnectHeader.TheOldConnectHeader != null)
                {
                    ConnectHeader.TheOldConnectHeader.ApplicationName = applicationName;
                }
            }
        }
    }
}
