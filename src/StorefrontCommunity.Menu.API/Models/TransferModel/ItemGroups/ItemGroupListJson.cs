using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class ItemGroupListJson : IActionResult
    {
        public ItemGroupListJson() { }

        public ItemGroupListJson(ICollection<ItemGroup> itemGroups, long count)
        {
            ItemGroups = itemGroups
                .Select(itemGroup => new ItemGroupJson(itemGroup))
                .ToList();

            Count = count;
        }

        public ICollection<ItemGroupJson> ItemGroups { get; set; }
        public long Count { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
