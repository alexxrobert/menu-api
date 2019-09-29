using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.ItemGroups
{
    public sealed class ItemGroupDeletedEvent : Event<ItemGroupPayload>
    {
        public ItemGroupDeletedEvent(ItemGroup itemGroup)
        {
            Name = "menu.item-group.deleted";
            Payload = new ItemGroupPayload(itemGroup);
        }
    }
}
