using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.EventModel.Published.ItemGroups;
using Storefront.Menu.API.Models.TransferModel.ItemGroups;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.ItemGroups
{
    public sealed class CreateItemGroupTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public CreateItemGroupTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldCreateSuccessfully()
        {
            var path = "/item-groups";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<ItemGroupJson>(response);
            var itemGroup = await _server.Database.ItemGroups.SingleAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_token.TenantId, itemGroup.TenantId);
            Assert.Equal(jsonRequest.Title, itemGroup.Title);
        }

        [Fact]
        public async Task ShouldPublishEventAfterCreateSuccessfully()
        {
            var path = "/item-groups";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var itemGroup = await _server.Database.ItemGroups.SingleAsync();
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.item-group.created");
            var payload = (ItemGroupPayload)publishedEvent.Payload;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Id, itemGroup.Id);
            Assert.Equal(payload.TenantId, itemGroup.TenantId);
            Assert.Equal(payload.Title, itemGroup.Title);
            Assert.Equal(payload.PictureFileId, itemGroup.PictureFileId);
        }
    }
}
