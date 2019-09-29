using System;
using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.OptionGroups
{
    public sealed class OptionGroupCreatedEvent : Event<OptionGroupPayload>
    {
        public OptionGroupCreatedEvent(OptionGroup optionGroup)
        {
            Name = "menu.option-group.created";
            Payload = new OptionGroupPayload(optionGroup);
        }
    }
}
