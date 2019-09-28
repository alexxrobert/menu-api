using System.Collections.Generic;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.DataModel.OptionGroups;

namespace Storefront.Menu.API.Models.DataModel.ItemGroups
{
    public sealed class ItemGroup
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string Title { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<OptionGroup> OptionGroups { get; set; }
    }
}
