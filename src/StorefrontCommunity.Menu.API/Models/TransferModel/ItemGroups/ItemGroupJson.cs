using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class ItemGroupJson : IActionResult
    {
        public ItemGroupJson() { }

        public ItemGroupJson(ItemGroup itemGroup)
        {
            Id = itemGroup.Id;
            Title = itemGroup.Title;
        }

        public long Id { get; set; }
        public string Title { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
