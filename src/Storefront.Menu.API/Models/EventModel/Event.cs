using System;

namespace Storefront.Menu.API.Models.EventModel
{
    public class Event<TPayload> : IEvent where TPayload : class
    {
        public Event()
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
