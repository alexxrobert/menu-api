using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.EventModel.Published.Options
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
