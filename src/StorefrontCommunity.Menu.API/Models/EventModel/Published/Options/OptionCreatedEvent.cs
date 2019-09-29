using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.Options
{
    public sealed class OptionCreatedEvent : Event<OptionPayload>
    {
        public OptionCreatedEvent(Option option)
        {
            Name = "menu.option.created";
            Payload = new OptionPayload(option);
        }
    }
}
