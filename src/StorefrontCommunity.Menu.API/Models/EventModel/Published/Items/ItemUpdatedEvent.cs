using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.Items
{
    public sealed class ItemUpdatedEvent : Event<ItemPayload>
    {
        public ItemUpdatedEvent(Item item)
        {
            Name = "menu.item.updated";
            Payload = new ItemPayload(item);
        }
    }
}
