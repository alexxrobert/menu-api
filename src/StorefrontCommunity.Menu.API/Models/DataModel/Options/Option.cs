using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;

namespace StorefrontCommunity.Menu.API.Models.DataModel.Options
{
    public sealed class Option
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public long OptionGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public OptionGroup OptionGroup { get; set; }
    }
}
