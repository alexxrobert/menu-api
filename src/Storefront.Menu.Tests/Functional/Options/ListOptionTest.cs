using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.TransferModel.Options;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.OptionGroups;
using Storefront.Menu.Tests.Factories.Options;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.Options
{
    public sealed class ListOptionTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public ListOptionTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldListAll()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup);

            var option1 = new Option().To(optionGroup);
            var option2 = new Option().To(optionGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.AddRange(option1, option2);

            await _server.Database.SaveChangesAsync();

            var path = "/options";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<OptionListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, jsonResponse.Count);
            Assert.Contains(jsonResponse.Options, json => json.Id == option1.Id);
            Assert.Contains(jsonResponse.Options, json => json.Id == option2.Id);
        }

        [Fact]
        public async Task ShouldListByName()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup);

            var option1 = new Option().To(optionGroup);
            var option2 = new Option().To(optionGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.AddRange(option1, option2);

            await _server.Database.SaveChangesAsync();

            var path = $"/options?name={option1.Name}";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<OptionListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Options, json => json.Id == option1.Id);
            Assert.DoesNotContain(jsonResponse.Options, json => json.Id == option2.Id);
        }

        [Fact]
        public async Task ShouldListAvailable()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup);

            var option1 = new Option().To(optionGroup, available: true);
            var option2 = new Option().To(optionGroup, available: false);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.AddRange(option1, option2);

            await _server.Database.SaveChangesAsync();

            var path = "/options?available=true";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<OptionListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Options, json => json.Id == option1.Id);
            Assert.DoesNotContain(jsonResponse.Options, json => json.Id == option2.Id);
        }

        [Fact]
        public async Task ShouldListNotAvailable()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            var option1 = new Option().To(optionGroup, available: false);
            var option2 = new Option().To(optionGroup, available: true);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.AddRange(option1, option2);

            await _server.Database.SaveChangesAsync();

            var path = "/options?available=false";
            var response = await _client.GetAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<OptionListJson>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, jsonResponse.Count);
            Assert.Contains(jsonResponse.Options, json => json.Id == option1.Id);
            Assert.DoesNotContain(jsonResponse.Options, json => json.Id == option2.Id);
        }
    }
}
