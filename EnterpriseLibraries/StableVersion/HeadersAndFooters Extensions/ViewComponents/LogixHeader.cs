using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LogixHealth.EnterpriseLibrary.Extensions.HeadersAndFooters.ViewComponents
{
    public class LogixHeader : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IViewComponentResult result = await Task.FromResult((IViewComponentResult)View("_LogixHeader", LogixHealth.EnterpriseLibrary.Extensions.HeadersAndFooters.LogixHeader.ConnectHeader));

            return result;
        }
    }
}
