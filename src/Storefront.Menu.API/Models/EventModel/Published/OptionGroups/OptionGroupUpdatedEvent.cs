using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.EventModel.Published.OptionGroups
{
    public sealed class OptionGroupUpdatedEvent : Event<OptionGroupPayload>
    {
        public OptionGroupUpdatedEvent(OptionGroup optionGroup)
        {
            Name = "menu.option-group.updated";
            Payload = new OptionGroupPayload(optionGroup);
        }
    }
}
