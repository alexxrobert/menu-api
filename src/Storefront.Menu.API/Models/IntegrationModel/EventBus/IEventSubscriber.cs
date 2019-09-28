using System.Threading.Tasks;

namespace Storefront.Menu.API.Models.IntegrationModel.EventBus
{
    public interface IEventSubscriber
    {
        Task Handle(string message);
    }
}
