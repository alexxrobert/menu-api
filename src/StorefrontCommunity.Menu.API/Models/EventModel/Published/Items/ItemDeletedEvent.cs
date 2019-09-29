using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.Items
{
    public sealed class ItemDeletedEvent : Event<ItemPayload>
    {
        public ItemDeletedEvent(Item item)
        {
            Name = "menu.item.deleted";
            Payload = new ItemPayload(item);
        }
    }
}
