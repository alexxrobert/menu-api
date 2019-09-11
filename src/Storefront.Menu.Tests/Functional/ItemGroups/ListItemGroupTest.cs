using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.TransferModel.ItemGroups;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.ItemGroups
{
    public sealed class ListItemGroupTest
    {
        private readonly ApiServer _server;

        public ListItemGroupTest()
        {
            _server = new ApiServer();
        }

        [Fact]
        public async Task ShouldListAll()
        {
            var token = new ApiToken(_server.JwtOptions);
            var client = new ApiClient(_server, token);

            var itemGroup1 = new ItemGroup().Of(token.TenantId);
            var itemGroup2 = new ItemGroup().Of(token.TenantId);

            _server.Database.ItemGroups.AddRange(itemGroup1, itemGroup2);
            await _server.Database.SaveChangesAsync();

            var path = "/item-groups";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemGroupListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(jsonResponse.ItemGroups, json => json.Id == itemGroup1.Id);
            Assert.Contains(jsonResponse.ItemGroups, json => json.Id == itemGroup2.Id);
        }

        [Fact]
        public async Task ShouldListByTitle()
        {
            var token = new ApiToken(_server.JwtOptions);
            var client = new ApiClient(_server, token);

            var itemGroup1 = new ItemGroup().Of(token.TenantId);
            var itemGroup2 = new ItemGroup().Of(token.TenantId);

            _server.Database.ItemGroups.AddRange(itemGroup1, itemGroup2);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups?title={itemGroup1.Title}";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<ItemGroupListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(jsonResponse.ItemGroups, json => json.Id == itemGroup1.Id);
            Assert.DoesNotContain(jsonResponse.ItemGroups, json => json.Id == itemGroup2.Id);
        }
    }
}
