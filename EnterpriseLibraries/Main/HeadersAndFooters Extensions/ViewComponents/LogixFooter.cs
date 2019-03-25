using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LogixHealth.EnterpriseLibrary.Extensions.HeadersAndFooters.ViewComponents
{
    public class LogixFooter : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IViewComponentResult result = await Task.FromResult((IViewComponentResult)View("_LogixFooter", LogixHealth.EnterpriseLibrary.Extensions.HeadersAndFooters.LogixFooter.ConnectFooter));

            return result;
        }
    }
}
