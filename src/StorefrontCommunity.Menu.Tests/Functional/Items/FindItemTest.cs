using System.Net;
using System.Threading.Tasks;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.TransferModel;
using StorefrontCommunity.Menu.API.Models.TransferModel.Items;
using StorefrontCommunity.Menu.Tests.Factories.ItemGroups;
using StorefrontCommunity.Menu.Tests.Factories.Items;
using StorefrontCommunity.Menu.Tests.Fakes;
using Xunit;

namespace StorefrontCommunity.Menu.Tests.Functional.Items
{
    public sealed class FindItemTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public FindItemTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldFindSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/items/{item.Id}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(item.Id, jsonResponse.Id);
            Assert.Equal(item.Name, jsonResponse.Name);
            Assert.Equal(item.Description, jsonResponse.Description);
            Assert.Equal(item.Price, jsonResponse.Price);
            Assert.Equal(item.IsAvailable, jsonResponse.Available);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = "/items/5";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_NOT_FOUND", jsonResponse.Error);
        }
    }
}
