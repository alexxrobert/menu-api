using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.OptionGroups
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
