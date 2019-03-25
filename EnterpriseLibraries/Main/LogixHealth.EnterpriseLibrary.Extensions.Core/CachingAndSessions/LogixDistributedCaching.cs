namespace LogixHealth.EnterpriseLibrary.Extensions.CachingAndSessions
{
    using Microsoft.Extensions.DependencyInjection;

    public static class LogixDistributedCaching
    {
        public static void AddLogixDistributedCaching(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();

            // services.AddDistributedSqlServerCache();
        }

        public static void AddLogixSession(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            double sessionIdleTimeout = System.Convert.ToDouble(configuration["ConnectAuthentication:SessionLifeTime"]);

            services.AddSession
                (
                    options =>
                    {
                        options.IdleTimeout = System.TimeSpan.FromSeconds(sessionIdleTimeout);
                    }
                );
        }
    }
}
