using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.EventModel.Published.Items;
using Storefront.Menu.API.Models.TransferModel;
using Storefront.Menu.API.Models.TransferModel.Items;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.Items;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.Items
{
    public sealed class UpdateItemTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public UpdateItemTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldUpdateSuccessfully()
        {
            var itemGroup1 = new ItemGroup().Of(_token.TenantId);
            var itemGroup2 = new ItemGroup().Of(_token.TenantId);
            var item = new Item().To(itemGroup1);

            _server.Database.ItemGroups.AddRange(itemGroup1, itemGroup2);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/items/{item.Id}";
            var jsonRequest = new SaveItemJson().Build(groupId: itemGroup2.Id);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<ItemJson>(response);

            await _server.Database.Entry(item).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_token.TenantId, item.TenantId);
            Assert.Equal(jsonRequest.GroupId, item.ItemGroupId);
            Assert.Equal(jsonRequest.Name, item.Name);
            Assert.Equal(jsonRequest.Description, item.Description);
            Assert.Equal(jsonRequest.Price, item.Price);
            Assert.Equal(jsonRequest.IsAvailable, item.IsAvailable);
            Assert.Equal(item.Id, jsonResponse.Id);
            Assert.Equal(item.ItemGroupId, itemGroup2.Id);
            Assert.Equal(item.Name, jsonResponse.Name);
            Assert.Equal(item.Description, jsonResponse.Description);
            Assert.Equal(item.Price, jsonResponse.Price);
            Assert.Equal(item.IsAvailable, jsonResponse.Available);
        }

        [Fact]
        public async Task ShouldPublishEventAfterUpdateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/items/{item.Id}";
            var jsonRequest = new SaveItemJson().Build(groupId: itemGroup.Id);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.item.updated");
            var payload = (ItemPayload)publishedEvent.Payload;

            await _server.Database.Entry(item).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Id, item.Id);
            Assert.Equal(payload.TenantId, item.TenantId);
            Assert.Equal(payload.Name, item.Name);
            Assert.Equal(payload.Description, item.Description);
            Assert.Equal(payload.Price, item.Price);
            Assert.Equal(payload.IsAvailable, item.IsAvailable);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);

            await _server.Database.SaveChangesAsync();

            var path = "/items/5";
            var jsonRequest = new SaveItemJson().Build(groupId: itemGroup.Id);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_NOT_FOUND", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentGroupId()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/items/{item.Id}";
            var jsonRequest = new SaveItemJson().Build(groupId: 80);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
