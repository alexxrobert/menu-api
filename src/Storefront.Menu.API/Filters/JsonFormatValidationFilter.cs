using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Storefront.Menu.API.Models.TransferModel.Errors;

namespace Storefront.Menu.API.Filters
{
    public sealed class JsonFormatValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
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
