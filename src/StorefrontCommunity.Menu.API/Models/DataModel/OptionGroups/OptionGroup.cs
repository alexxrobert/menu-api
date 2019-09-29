using System.Collections.Generic;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.Options;

namespace StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups
{
    public sealed class OptionGroup
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public long ItemGroupId { get; set; }
        public string Title { get; set; }
        public bool IsMultichoice { get; set; }
        public bool IsRequired { get; set; }
        public ItemGroup ItemGroup { get; set; }
        public ICollection<Option> Options { get; set; }
    }
}
