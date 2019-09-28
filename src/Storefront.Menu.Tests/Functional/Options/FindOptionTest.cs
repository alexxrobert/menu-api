using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.TransferModel;
using Storefront.Menu.API.Models.TransferModel.Options;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.OptionGroups;
using Storefront.Menu.Tests.Factories.Options;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.Options
{
    public sealed class FindOptionTest
    {
        private readonly FakeApiServer _server;

        public FindOptionTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldFindSuccessfully()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.Add(option);

            await _server.Database.SaveChangesAsync();

            var path = $"/options/{option.Id}";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<OptionJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(option.Id, jsonResponse.Id);
            Assert.Equal(option.Name, jsonResponse.Name);
            Assert.Equal(option.Description, jsonResponse.Description);
            Assert.Equal(option.Price, jsonResponse.Price);
            Assert.Equal(option.IsAvailable, jsonResponse.Available);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var path = "/options/5";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_NOT_FOUND", jsonResponse.Error);
        }
    }
}
