using System.Net;
using System.Threading.Tasks;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.TransferModel;
using StorefrontCommunity.Menu.API.Models.TransferModel.ItemGroups;
using StorefrontCommunity.Menu.Tests.Factories.ItemGroups;
using StorefrontCommunity.Menu.Tests.Fakes;
using Xunit;

namespace StorefrontCommunity.Menu.Tests.Functional.ItemGroups
{
    public sealed class FindItemGroupTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public FindItemGroupTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldFindSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemGroupJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(itemGroup.Id, jsonResponse.Id);
            Assert.Equal(itemGroup.Title, jsonResponse.Title);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var path = "/item-groups/5";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
