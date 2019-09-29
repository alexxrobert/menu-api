using System.Collections.Generic;
using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;

namespace StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups
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
