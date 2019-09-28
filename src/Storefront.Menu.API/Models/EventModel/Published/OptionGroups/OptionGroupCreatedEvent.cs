using System;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.EventModel.Published.OptionGroups
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
