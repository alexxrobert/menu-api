using Storefront.Menu.API.Models.DataModel.Items;

namespace Storefront.Menu.API.Models.EventModel.Published.Items
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
