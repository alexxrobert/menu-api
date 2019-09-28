using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.EventModel.Published.OptionGroups;
using Storefront.Menu.API.Models.TransferModel.OptionGroups;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.OptionGroups;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.OptionGroups
{
    public sealed class CreateOptionGroupTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public CreateOptionGroupTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldCreateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = "/option-groups";
            var jsonRequest = new SaveOptionGroupJson().Build();
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<OptionGroupJson>(response);
            var optionGroup = await _server.Database.OptionGroups.SingleAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_token.TenantId, optionGroup.TenantId);
            Assert.Equal(jsonRequest.Title, optionGroup.Title);
        }

        [Fact]
        public async Task ShouldPublishEventAfterCreateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = "/option-groups";
            var jsonRequest = new SaveOptionGroupJson().Build();
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var optionGroup = await _server.Database.OptionGroups.SingleAsync();
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.option-group.created");
            var payload = (OptionGroupPayload)publishedEvent.Payload;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Id, optionGroup.Id);
            Assert.Equal(payload.TenantId, optionGroup.TenantId);
            Assert.Equal(payload.Title, optionGroup.Title);
        }
    }
}
