namespace StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus
{
    public interface IMessageBroker
    {
        void Publish<TPayload>(Event<TPayload> @event);
    }
}
