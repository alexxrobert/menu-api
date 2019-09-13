using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.EventModel.Published.ItemGroups
{
    public sealed class ItemGroupUpdatedEvent : IntegrationEvent<ItemGroupPayload>
    {
        public ItemGroupUpdatedEvent(ItemGroup itemGroup)
        {
            Name = "menu.itemgroup.updated";
            Payload = new ItemGroupPayload(itemGroup);
        }
    }
}
