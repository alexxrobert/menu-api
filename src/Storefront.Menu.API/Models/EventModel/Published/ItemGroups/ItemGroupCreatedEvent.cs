using System;
using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.EventModel.Published.ItemGroups
{
    public sealed class ItemGroupCreatedEvent : Event<ItemGroupPayload>
    {
        public ItemGroupCreatedEvent(ItemGroup itemGroup)
        {
            Name = "menu.itemgroup.created";
            Payload = new ItemGroupPayload(itemGroup);
        }
    }
}
