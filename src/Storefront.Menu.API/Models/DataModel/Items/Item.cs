using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.DataModel.Items
{
    public sealed class Item
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public long ItemGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureFileId { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public ItemGroup ItemGroup { get; set; }
    }
}
