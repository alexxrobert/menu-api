using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.EventModel.Published.Items;
using Storefront.Menu.API.Models.TransferModel;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.Items;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.Items
{
    public sealed class DeleteItemTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public DeleteItemTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/items/{item.Id}";
            var response = await _client.DeleteAsync(path);
            var hasBeenDeleted = !await _server.Database.Items
                .WhereKey(_token.TenantId, item.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task ShouldPublishEventAfterDeleteSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/items/{item.Id}";
            var response = await _client.DeleteAsync(path);
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.item.deleted");
            var payload = (ItemPayload)publishedEvent.Payload;

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
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
            var response = await _client.DeleteAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_NOT_FOUND", jsonResponse.Error);
        }
    }
}
