using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.EventModel.Published.OptionGroups;
using Storefront.Menu.API.Models.TransferModel;
using Storefront.Menu.API.Models.TransferModel.OptionGroups;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.OptionGroups;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.OptionGroups
{
    public sealed class UpdateOptionGroupTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public UpdateOptionGroupTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldUpdateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/option-groups/{optionGroup.Id}";
            var jsonRequest = new SaveOptionGroupJson().Build();
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<OptionGroupJson>(response);

            await _server.Database.Entry(optionGroup).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_token.TenantId, optionGroup.TenantId);
            Assert.Equal(jsonRequest.Title, optionGroup.Title);
        }

        [Fact]
        public async Task ShouldPublishEventAfterUpdateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/option-groups/{optionGroup.Id}";
            var jsonRequest = new SaveOptionGroupJson().Build();
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.option-group.updated");
            var payload = (OptionGroupPayload)publishedEvent.Payload;

            await _server.Database.Entry(optionGroup).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Id, optionGroup.Id);
            Assert.Equal(payload.TenantId, optionGroup.TenantId);
            Assert.Equal(payload.Title, optionGroup.Title);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = "/option-groups/5";
            var jsonRequest = new SaveOptionGroupJson().Build();
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
