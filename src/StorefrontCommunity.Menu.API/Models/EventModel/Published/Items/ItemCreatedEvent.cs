using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.Items
{
    public sealed class ItemCreatedEvent : Event<ItemPayload>
    {
        public ItemCreatedEvent(Item item)
        {
            Name = "menu.item.created";
            Payload = new ItemPayload(item);
        }
    }
}
