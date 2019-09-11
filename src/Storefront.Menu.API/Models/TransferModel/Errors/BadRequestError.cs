using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Storefront.Menu.API.Models.TransferModel.Errors
{
    public sealed class BadRequestError : IActionResult
    {
        public BadRequestError() { }

        public BadRequestError(params string[] errors)
        {
            Errors = errors;
        }

        public BadRequestError(IEnumerable<JsonFormatErrorMessage> errorMessages)
        {
            Errors = errorMessages.Select(errorMessage => errorMessage.ToString());
        }

        public IEnumerable<string> Errors { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var json = new JsonResult(this) { StatusCode = 400 };
            await json.ExecuteResultAsync(context);
        }
    }
}
