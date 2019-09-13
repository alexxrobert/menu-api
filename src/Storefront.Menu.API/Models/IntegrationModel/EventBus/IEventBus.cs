using Storefront.Menu.API.Models.EventModel;

namespace Storefront.Menu.API.Models.IntegrationModel.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent @event);
    }
}
