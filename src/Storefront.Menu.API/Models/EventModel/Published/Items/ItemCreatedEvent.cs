using System;
using Storefront.Menu.API.Models.DataModel.Items;

namespace Storefront.Menu.API.Models.EventModel.Published.Items
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