using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.EventModel.Published.OptionGroups
{
    public sealed class OptionGroupDeletedEvent : Event<OptionGroupPayload>
    {
        public OptionGroupDeletedEvent(OptionGroup optionGroup)
        {
            Name = "menu.option-group.deleted";
            Payload = new OptionGroupPayload(optionGroup);
        }
    }
}
