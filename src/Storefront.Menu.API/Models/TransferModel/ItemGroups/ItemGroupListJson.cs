using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class ItemGroupListJson : IActionResult
    {
        public ItemGroupListJson() { }

        public ItemGroupListJson(ICollection<ItemGroup> itemGroups)
        {
            ItemGroups = itemGroups
                .Select(itemGroup => new ItemGroupJson(itemGroup))
                .ToList();
        }

        public ICollection<ItemGroupJson> ItemGroups { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
