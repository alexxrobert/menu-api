using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.EventModel.Published.Options
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
