using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Storefront.Menu.API.Models.TransferModel.Errors;

namespace Storefront.Menu.API.Filters
{
    public sealed class RequestBodyValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments.Any(arg => arg.Value == null))
            {
                filterContext.Result = new BadRequestError("Request body cannot be blank.");
            }
            else if (!filterContext.ModelState.IsValid)
            {
                var errors = filterContext.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => new JsonFormatErrorMessage(e))
                    .ToList();

                filterContext.Result = new BadRequestError(errors);
            }
        }
    }
}
