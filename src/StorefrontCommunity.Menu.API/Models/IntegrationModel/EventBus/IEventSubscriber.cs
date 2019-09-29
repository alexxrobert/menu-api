using System.Threading.Tasks;

namespace StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus
{
    public interface IEventSubscriber
    {
        Task Handle(string message);
    }
}
