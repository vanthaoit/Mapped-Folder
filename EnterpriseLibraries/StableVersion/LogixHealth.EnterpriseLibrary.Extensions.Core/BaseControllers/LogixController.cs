using LogixHealth.EnterpriseLibrary.Extensions.Authentication;

namespace LogixHealth.EnterpriseLibrary.Extensions.BaseControllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class LogixController : Microsoft.AspNetCore.Mvc.Controller
    {
        public Microsoft.AspNetCore.Mvc.IActionResult SignOut()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return this.SignIn
                    (
                        (HttpContext?.User),
                        Microsoft.AspNetCore.Authentication.WsFederation.WsFederationDefaults.AuthenticationScheme
                    );
            }

            foreach (var key in this.HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(key);
            }

            return this.SignOut
                (
                    new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                    {
                        RedirectUri = HttpContext.User.AudienceUrl()
                    },
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    Microsoft.AspNetCore.Authentication.WsFederation.WsFederationDefaults.AuthenticationScheme
                );
        }
    }
}

