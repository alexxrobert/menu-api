using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storefront.Menu.API.Models.DataModel.Items;

namespace Storefront.Menu.API.Models.TransferModel.Items
{
    public sealed class ItemListJson : IActionResult
    {
        public ItemListJson() { }

        public ItemListJson(ICollection<Item> items, long count)
        {
            Items = items
                .Select(item => new ItemJson(item))
                .ToList();

            Count = count;
        }

        public ICollection<ItemJson> Items { get; set; }
        public long Count { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
