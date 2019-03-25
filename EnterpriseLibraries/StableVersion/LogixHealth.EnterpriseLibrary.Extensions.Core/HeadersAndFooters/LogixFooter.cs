namespace LogixHealth.EnterpriseLibrary.Extensions.HeadersAndFooters
{
    using System.Linq;

    public class LogixConnectFooter
    {
        public LogixHealth.Connect.BusinessEntities.ApplicationFooter TheNewConnectFooter { get; set; }
    }

    public static class LogixFooter
    {
        public static LogixConnectFooter ConnectFooter { get; set; }

        public static void AddLogixAppFooter(this Microsoft.AspNetCore.Builder.IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            if (Authentication.LogixAuthentication.LogixUser == null) return;

            if (Authentication.LogixAuthentication.LogixUser.Claims.Count() == 0) return;

            string applicationName = configuration["ApplicationName"];
            string endpoint = configuration["HeaderAndFooter:Address"];
            string binding = configuration["HeaderAndFooter:Binding"];

            LogixHeaderAndFooterServiceClient serviceClient = new LogixHeaderAndFooterServiceClient(binding, endpoint);
            ConnectFooter = new LogixConnectFooter
            {
                TheNewConnectFooter = serviceClient.GetUserFooterCurrentArea(applicationName)
            };
        }
    }
}
