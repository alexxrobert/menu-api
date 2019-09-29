using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.Options
{
    public sealed class OptionUpdatedEvent : Event<OptionPayload>
    {
        public OptionUpdatedEvent(Option option)
        {
            Name = "menu.option.updated";
            Payload = new OptionPayload(option);
        }
    }
}
