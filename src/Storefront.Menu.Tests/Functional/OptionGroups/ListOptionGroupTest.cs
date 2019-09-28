using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.TransferModel.OptionGroups;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.OptionGroups;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.OptionGroups
{
    public sealed class ListOptionGroupTest
    {
        private readonly FakeApiServer _server;

        public ListOptionGroupTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldListAll()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var optionGroup1 = new OptionGroup().To(itemGroup);
            var optionGroup2 = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.AddRange(optionGroup1, optionGroup2);
            await _server.Database.SaveChangesAsync();

            var path = "/option-groups";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<OptionGroupListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, jsonResponse.Count);
            Assert.Contains(jsonResponse.OptionGroups, json => json.Id == optionGroup1.Id);
            Assert.Contains(jsonResponse.OptionGroups, json => json.Id == optionGroup2.Id);
        }

        [Fact]
        public async Task ShouldListByTitle()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var optionGroup1 = new OptionGroup().To(itemGroup);
            var optionGroup2 = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.AddRange(optionGroup1, optionGroup2);
            await _server.Database.SaveChangesAsync();

            var path = $"/option-groups?title={optionGroup1.Title}";
            var response = await client.GetAsync(path);
            var jsonResponse = await client.ReadJsonAsync<OptionGroupListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.OptionGroups, json => json.Id == optionGroup1.Id);
            Assert.DoesNotContain(jsonResponse.OptionGroups, json => json.Id == optionGroup2.Id);
        }
    }
}
