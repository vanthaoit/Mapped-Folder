namespace LogixHealth.EnterpriseLibrary.Extensions.AspNetMvcInitializer
{
    using Microsoft.Extensions.DependencyInjection;

    public static class LogixAspNetMvcInitializer
    {
        public static void InitializeAspNetCoreContexts(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddSingleton(provider => configuration);
        }
    }
}
