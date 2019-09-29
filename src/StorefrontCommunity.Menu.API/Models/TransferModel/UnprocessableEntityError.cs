using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StorefrontCommunity.Menu.API.Models.TransferModel
{
    public class UnprocessableEntityError : IActionResult
    {
        public UnprocessableEntityError() { }

        public string Error { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var json = new JsonResult(this) { StatusCode = 422 };
            await json.ExecuteResultAsync(context);
        }
    }
}
