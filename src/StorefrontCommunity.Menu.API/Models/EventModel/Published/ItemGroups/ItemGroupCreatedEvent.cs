using System;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.ItemGroups
{
    public sealed class ItemGroupCreatedEvent : Event<ItemGroupPayload>
    {
        public ItemGroupCreatedEvent(ItemGroup itemGroup)
        {
            Name = "menu.item-group.created";
            Payload = new ItemGroupPayload(itemGroup);
        }
    }
}
