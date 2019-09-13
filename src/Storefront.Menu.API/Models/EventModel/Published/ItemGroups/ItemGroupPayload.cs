using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.EventModel.Published.ItemGroups
{
    public class ItemGroupPayload
    {
        public ItemGroupPayload(ItemGroup itemGroup)
        {
            Id = itemGroup.Id;
            TenantId = itemGroup.TenantId;
            Title = itemGroup.Title;
            PictureFileId = itemGroup.PictureFileId;
        }

        public long Id { get; }
        public long TenantId { get; }
        public string Title { get; }
        public string PictureFileId { get; }
    }
}
