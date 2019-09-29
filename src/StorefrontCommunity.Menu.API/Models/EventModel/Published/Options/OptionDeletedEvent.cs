using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.EventModel.Published.Options
{
    public sealed class OptionDeletedEvent : Event<OptionPayload>
    {
        public OptionDeletedEvent(Option option)
        {
            Name = "menu.option.deleted";
            Payload = new OptionPayload(option);
        }
    }
}
