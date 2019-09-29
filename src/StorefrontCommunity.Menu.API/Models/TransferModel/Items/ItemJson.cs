using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorefrontCommunity.Menu.API.Models.DataModel.Items;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.Items
{
    public sealed class ItemJson : IActionResult
    {
        public ItemJson() { }

        public ItemJson(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Available = item.IsAvailable;
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
