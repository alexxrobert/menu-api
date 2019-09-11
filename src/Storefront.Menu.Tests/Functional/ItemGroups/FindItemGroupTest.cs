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
    public sealed class FindItemGroupTest
    {
        private readonly ApiServer _server;

        public FindItemGroupTest()
        {
            _server = new ApiServer();
        }

        [Fact]
        public async Task ShouldFindSuccessfully()
        {
            var token = new ApiToken(_server.JwtOptions);
            var client = new ApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemGroupJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(itemGroup.Id, jsonResponse.Id);
            Assert.Equal(itemGroup.Title, jsonResponse.Title);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var token = new ApiToken(_server.JwtOptions);
            var client = new ApiClient(_server, token);

            var path = "/item-groups/5";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
