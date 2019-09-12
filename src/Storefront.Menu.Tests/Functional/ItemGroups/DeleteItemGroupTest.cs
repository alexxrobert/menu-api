using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.TransferModel.Errors;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.Items;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.ItemGroups
{
    public sealed class DeleteItemGroupTest
    {
        private readonly FakeApiServer _server;

        public DeleteItemGroupTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}";
            var response = await client.DeleteAsync(path);
            var hasBeenDeleted = !await _server.Database.ItemGroups
                .WhereId(itemGroup.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var path = "/item-groups/5";
            var response = await client.DeleteAsync(path);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_GROUP_NOT_FOUND", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond422IfGroupHasItems()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var item = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}";
            var response = await client.DeleteAsync(path);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);
            var hasBeenDeleted = !await _server.Database.ItemGroups
                .WhereId(itemGroup.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_GROUP_HAS_ITEMS", jsonResponse.Error);
            Assert.False(hasBeenDeleted);
        }
    }
}
