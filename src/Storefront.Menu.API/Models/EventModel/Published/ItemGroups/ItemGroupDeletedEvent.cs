using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.EventModel.Published.ItemGroups
{
    public sealed class ItemGroupDeletedEvent : Event<ItemGroupPayload>
    {
        public ItemGroupDeletedEvent(ItemGroup itemGroup)
        {
            Name = "menu.itemgroup.deleted";
            Payload = new ItemGroupPayload(itemGroup);
        }
    }
}
