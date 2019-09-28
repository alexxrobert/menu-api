using Storefront.Menu.API.Models.DataModel.Options;

namespace Storefront.Menu.API.Models.EventModel.Published.Options
{
    public class OptionPayload
    {
        public OptionPayload(Option option)
        {
            Id = option.Id;
            TenantId = option.TenantId;
            Name = option.Name;
            Description = option.Description;
            Price = option.Price;
            IsAvailable = option.IsAvailable;
        }

        public long Id { get; }
        public long TenantId { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
