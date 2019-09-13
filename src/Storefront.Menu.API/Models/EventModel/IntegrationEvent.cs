using System;

namespace Storefront.Menu.API.Models.EventModel
{
    public class IntegrationEvent<TPayload> : IEvent where TPayload : class
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public TPayload Payload { get; set; }
        object IEvent.Payload => Payload;
    }
}
