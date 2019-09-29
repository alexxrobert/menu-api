using System.Net;
using System.Threading.Tasks;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.TransferModel.ItemGroups;
using StorefrontCommunity.Menu.Tests.Factories.ItemGroups;
using StorefrontCommunity.Menu.Tests.Fakes;
using Xunit;

namespace StorefrontCommunity.Menu.Tests.Functional.ItemGroups
{
    public sealed class ListItemGroupTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public ListItemGroupTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldListAll()
        {
            var itemGroup1 = new ItemGroup().Of(_token.TenantId);
            var itemGroup2 = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.AddRange(itemGroup1, itemGroup2);
            await _server.Database.SaveChangesAsync();

            var path = "/item-groups";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemGroupListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, jsonResponse.Count);
            Assert.Contains(jsonResponse.ItemGroups, json => json.Id == itemGroup1.Id);
            Assert.Contains(jsonResponse.ItemGroups, json => json.Id == itemGroup2.Id);
        }

        [Fact]
        public async Task ShouldListByTitle()
        {
            var itemGroup1 = new ItemGroup().Of(_token.TenantId);
            var itemGroup2 = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.AddRange(itemGroup1, itemGroup2);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups?title={itemGroup1.Title}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemGroupListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.ItemGroups, json => json.Id == itemGroup1.Id);
            Assert.DoesNotContain(jsonResponse.ItemGroups, json => json.Id == itemGroup2.Id);
        }
    }
}
