using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.TransferModel.ItemGroups;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.ItemGroups
{
    public sealed class CreateItemGroupTest
    {
        private readonly FakeApiServer _server;

        public CreateItemGroupTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldCreateSuccessfully()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var path = "/item-groups";
            var jsonRequest = new SaveItemGroupJson().Build();
            var response = await client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await client.ReadJsonAsync<ItemGroupJson>(response);
            var itemGroup = await _server.Database.ItemGroups.SingleAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(token.TenantId, itemGroup.TenantId);
            Assert.Equal(jsonRequest.Title, itemGroup.Title);
        }
    }
}
