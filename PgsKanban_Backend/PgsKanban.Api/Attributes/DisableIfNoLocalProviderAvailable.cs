using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PgsKanban.BusinessLogic.Enums;
using PgsKanban.BusinessLogic.Managers;

namespace PgsKanban.Api.Attributes
{
    public class DisableIfNoLocalProviderAvailable : ActionFilterAttribute
    {
        private readonly IExternalLoginProviderManager _loginProviderManager;
        public DisableIfNoLocalProviderAvailable(IExternalLoginProviderManager loginProviderManager)
        {
            _loginProviderManager = loginProviderManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_loginProviderManager.IsProviderAvailable(ExternalLoginProvider.Local))
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
