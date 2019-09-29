using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.ItemGroups
{
    public class ItemGroupPayload
    {
        public ItemGroupPayload(ItemGroup itemGroup)
        {
            Id = itemGroup.Id;
            TenantId = itemGroup.TenantId;
            Title = itemGroup.Title;
        }

        public long Id { get; }
        public long TenantId { get; }
        public string Title { get; }
    }
}
