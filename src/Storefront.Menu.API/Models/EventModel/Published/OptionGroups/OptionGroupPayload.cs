using Storefront.Menu.API.Models.DataModel.OptionGroups;

namespace Storefront.Menu.API.Models.EventModel.Published.OptionGroups
{
    public class OptionGroupPayload
    {
        public OptionGroupPayload(OptionGroup optionGroup)
        {
            Id = optionGroup.Id;
            TenantId = optionGroup.TenantId;
            Title = optionGroup.Title;
        }

        public long Id { get; }
        public long TenantId { get; }
        public string Title { get; }
    }
}
