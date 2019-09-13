using System;

namespace Storefront.Menu.API.Models.EventModel
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
        string Name { get; }
        object Payload { get; }
    }
}
