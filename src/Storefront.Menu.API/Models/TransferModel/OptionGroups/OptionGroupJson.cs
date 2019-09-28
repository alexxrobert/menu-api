using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storefront.Menu.API.Models.DataModel.OptionGroups;

namespace Storefront.Menu.API.Models.TransferModel.OptionGroups
{
    public sealed class OptionGroupJson : IActionResult
    {
        public OptionGroupJson() { }

        public OptionGroupJson(OptionGroup optionGroup)
        {
            Id = optionGroup.Id;
            Title = optionGroup.Title;
        }

        public long Id { get; set; }
        public string Title { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
