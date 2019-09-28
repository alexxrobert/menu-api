using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.TransferModel.Items;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.Items;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.Items
{
    public sealed class ListItemTest
    {
        private readonly FakeApiServer _server;

        public ListItemTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldListAll()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var item1 = new Item().To(itemGroup);
            var item2 = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = "/items";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.Contains(jsonResponse.Items, json => json.Id == item2.Id);
        }

        [Fact]
        public async Task ShouldListByName()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var item1 = new Item().To(itemGroup);
            var item2 = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = $"/items?name={item1.Name}";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.DoesNotContain(jsonResponse.Items, json => json.Id == item2.Id);
        }

        [Fact]
        public async Task ShouldListAvailable()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var item1 = new Item().To(itemGroup, available: true);
            var item2 = new Item().To(itemGroup, available: false);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = "/items?available=true";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.DoesNotContain(jsonResponse.Items, json => json.Id == item2.Id);
        }

        [Fact]
        public async Task ShouldListNotAvailable()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var item1 = new Item().To(itemGroup, available: false);
            var item2 = new Item().To(itemGroup, available: true);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.AddRange(item1, item2);

            await _server.Database.SaveChangesAsync();

            var path = "/items?available=false";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Items, json => json.Id == item1.Id);
            Assert.DoesNotContain(jsonResponse.Items, json => json.Id == item2.Id);
        }
    }
}
