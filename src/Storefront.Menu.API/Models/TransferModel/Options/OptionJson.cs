using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storefront.Menu.API.Models.DataModel.Options;

namespace Storefront.Menu.API.Models.TransferModel.Options
{
    public sealed class OptionJson : IActionResult
    {
        public OptionJson() { }

        public OptionJson(Option option)
        {
            Id = option.Id;
            Name = option.Name;
            Description = option.Description;
            Price = option.Price;
            Available = option.IsAvailable;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
