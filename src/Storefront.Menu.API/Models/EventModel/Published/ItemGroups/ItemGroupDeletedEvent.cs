using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.EventModel.Published.ItemGroups
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
