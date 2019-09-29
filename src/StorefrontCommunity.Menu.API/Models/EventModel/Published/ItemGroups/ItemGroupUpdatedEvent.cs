using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.ItemGroups
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
