using System.Net;
using System.Threading.Tasks;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.TransferModel.Items;
using StorefrontCommunity.Menu.Tests.Factories.ItemGroups;
using StorefrontCommunity.Menu.Tests.Factories.Items;
using StorefrontCommunity.Menu.Tests.Fakes;
using Xunit;

namespace StorefrontCommunity.Menu.Tests.Functional.Items
{
    public sealed class ListItemTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public ListItemTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldListAll()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item1 = new Item().To(itemGroup);
            var item2 = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = "/items";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.Contains(jsonResponse.Items, json => json.Id == item2.Id);
        }

        [Fact]
        public async Task ShouldListByName()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item1 = new Item().To(itemGroup);
            var item2 = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = $"/items?name={item1.Name}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.DoesNotContain(jsonResponse.Items, json => json.Id == item2.Id);
        }

        [Fact]
        public async Task ShouldListAvailable()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item1 = new Item().To(itemGroup, available: true);
            var item2 = new Item().To(itemGroup, available: false);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = "/items?available=true";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.DoesNotContain(jsonResponse.Items, json => json.Id == item2.Id);
        }

        [Fact]
        public async Task ShouldListNotAvailable()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var item1 = new Item().To(itemGroup, available: false);
            var item2 = new Item().To(itemGroup, available: true);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = "/items?available=false";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.DoesNotContain(jsonResponse.Items, json => json.Id == item2.Id);
        }
    }
}
