using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.TransferModel.Errors;
using Storefront.Menu.API.Models.TransferModel.Items;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.Items;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.Items
{
    public sealed class FindItemTest
    {
        private readonly FakeApiServer _server;

        public FindItemTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldFindSuccessfully()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var item = new Item().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.Items.Add(item);

            await _server.Database.SaveChangesAsync();

            var path = $"/items/{item.Id}";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemJson>(response);

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
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var path = "/items/5";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("ITEM_NOT_FOUND", jsonResponse.Error);
        }
    }
}
