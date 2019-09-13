using System.Collections.Generic;
using Storefront.Menu.API.Models.EventModel;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.Tests.Fakes
{
    public sealed class FakeEventBus : IEventBus
    {
        public FakeEventBus()
        {
            PublishedEvents = new List<IEvent>();
        }

        public ICollection<IEvent> PublishedEvents { get; }

        public void Publish(IEvent @event)
        {
            PublishedEvents.Add(@event);
        }
    }
}
