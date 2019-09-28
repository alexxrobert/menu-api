using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.EventModel.Published.Options
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
