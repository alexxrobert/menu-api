using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.EventModel.Published.ItemGroups;
using Storefront.Menu.API.Models.TransferModel.Errors;
using Storefront.Menu.API.Models.TransferModel.ItemGroups;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.ItemGroups
{
    public sealed class UpdateItemGroupTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public UpdateItemGroupTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldUpdateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<ItemGroupJson>(response);

            await _server.Database.Entry(itemGroup).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_token.TenantId, itemGroup.TenantId);
            Assert.Equal(jsonRequest.Title, itemGroup.Title);
        }

        [Fact]
        public async Task ShouldPublishEventAfterUpdateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.item-group.updated");
            var payload = (ItemGroupPayload)publishedEvent.Payload;

            await _server.Database.Entry(itemGroup).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Id, itemGroup.Id);
            Assert.Equal(payload.TenantId, itemGroup.TenantId);
            Assert.Equal(payload.Title, itemGroup.Title);
            Assert.Equal(payload.PictureFileId, itemGroup.PictureFileId);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var path = "/item-groups/5";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
