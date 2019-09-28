using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.EventModel.Published.Options;
using Storefront.Menu.API.Models.TransferModel;
using Storefront.Menu.API.Models.TransferModel.Options;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.OptionGroups;
using Storefront.Menu.Tests.Factories.Options;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.Options
{
    public sealed class CreateOptionTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public CreateOptionTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldCreateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            await _server.Database.SaveChangesAsync();

            var path = "/options";
            var jsonRequest = new SaveOptionJson().Build(groupId: optionGroup.Id);
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<OptionJson>(response);
            var option = await _server.Database.Options.SingleAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_token.TenantId, option.TenantId);
            Assert.Equal(jsonRequest.Name, option.Name);
            Assert.Equal(jsonRequest.Description, option.Description);
            Assert.Equal(jsonRequest.Price, option.Price);
            Assert.Equal(jsonRequest.IsAvailable, option.IsAvailable);
            Assert.Equal(option.Id, jsonResponse.Id);
            Assert.Equal(option.Name, jsonResponse.Name);
            Assert.Equal(option.Description, jsonResponse.Description);
            Assert.Equal(option.Price, jsonResponse.Price);
            Assert.Equal(option.IsAvailable, jsonResponse.Available);
        }

        [Fact]
        public async Task ShouldPublishEventAfterCreateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            await _server.Database.SaveChangesAsync();

            var path = "/options";
            var jsonRequest = new SaveOptionJson().Build(groupId: optionGroup.Id);
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var option = await _server.Database.Options.SingleAsync();
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.option.created");
            var payload = (OptionPayload)publishedEvent.Payload;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Id, option.Id);
            Assert.Equal(payload.TenantId, option.TenantId);
            Assert.Equal(payload.Name, option.Name);
            Assert.Equal(payload.Description, option.Description);
            Assert.Equal(payload.Price, option.Price);
            Assert.Equal(payload.IsAvailable, option.IsAvailable);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentGroupId()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            await _server.Database.SaveChangesAsync();

            var path = "/options";
            var jsonRequest = new SaveOptionJson().Build(groupId: 90);
            var response = await _client.PostJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
