using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.EventModel.Published.ItemGroups
{
    public sealed class ItemGroupUpdatedEvent : Event<ItemGroupPayload>
    {
        public ItemGroupUpdatedEvent(ItemGroup itemGroup)
        {
            Name = "menu.item-group.updated";
            Payload = new ItemGroupPayload(itemGroup);
        }
    }
}
