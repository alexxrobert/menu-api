using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class ItemGroupJson : IActionResult
    {
        public ItemGroupJson() { }

        public ItemGroupJson(ItemGroup itemGroup)
        {
            Id = itemGroup.Id;
            Title = itemGroup.Title;
            ImageUrl = "";
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
