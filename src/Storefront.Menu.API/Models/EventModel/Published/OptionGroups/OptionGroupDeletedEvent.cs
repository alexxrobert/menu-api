using Storefront.Menu.API.Models.DataModel.OptionGroups;

namespace Storefront.Menu.API.Models.EventModel.Published.OptionGroups
{
    public sealed class OptionGroupDeletedEvent : Event<OptionGroupPayload>
    {
        public OptionGroupDeletedEvent(OptionGroup optionGroup)
        {
            Name = "menu.optiongroup.deleted";
            Payload = new OptionGroupPayload(optionGroup);
        }
    }
}
