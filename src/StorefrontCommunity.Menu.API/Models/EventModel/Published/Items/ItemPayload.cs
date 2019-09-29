using StorefrontCommunity.Menu.API.Models.DataModel.Items;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.Items
{
    public class ItemPayload
    {
        public ItemPayload(Item item)
        {
            Id = item.Id;
            TenantId = item.TenantId;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            IsAvailable = item.IsAvailable;
        }

        public long Id { get; }
        public long TenantId { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
