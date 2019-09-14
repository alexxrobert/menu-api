using Storefront.Menu.API.Models.DataModel.Items;

namespace Storefront.Menu.API.Models.EventModel.Published.Items
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
