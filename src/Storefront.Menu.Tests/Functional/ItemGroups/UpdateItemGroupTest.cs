using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
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

        public UpdateItemGroupTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldUpdateSuccessfully()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadJsonAsync<ItemGroupJson>(response);

            await _server.Database.Entry(itemGroup).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(token.TenantId, itemGroup.TenantId);
            Assert.Equal(jsonRequest.Title, itemGroup.Title);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var path = "/item-groups/5";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
