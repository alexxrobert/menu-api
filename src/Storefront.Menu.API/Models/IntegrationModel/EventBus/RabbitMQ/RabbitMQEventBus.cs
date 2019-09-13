using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Storefront.Menu.API.Models.EventModel;

namespace Storefront.Menu.API.Models.IntegrationModel.EventBus.RabbitMQ
{
    [ExcludeFromCodeCoverage]
    public sealed class RabbitMQEventBus : IEventBus
    {
        private readonly RabbitMQOptions _options;

        public RabbitMQEventBus(IOptions<RabbitMQOptions> options)
        {
            _options = options.Value;
        }

        public void Publish(IEvent @event)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_options.Host)
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: _options.Exchange,
                    type: "topic",
                    durable: false,
                    autoDelete: true,
                    arguments: null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: _options.Exchange,
                    routingKey: @event.Name,
                    mandatory: false,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
