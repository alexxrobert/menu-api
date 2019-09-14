using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Storefront.Menu.API.Models.TransferModel.Errors;

namespace Storefront.Menu.API.Filters
{
    public sealed class EmptyRequestBodyValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments.Any(arg => arg.Value == null))
            {
                filterContext.Result = new BadRequestError("Request body cannot be blank.");
            }
        }
    }
}
