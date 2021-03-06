using System.Linq;
using System.Net;
using System.Threading.Tasks;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.EventModel.Published.Options;
using StorefrontCommunity.Menu.API.Models.TransferModel;
using StorefrontCommunity.Menu.API.Models.TransferModel.Options;
using StorefrontCommunity.Menu.Tests.Factories.ItemGroups;
using StorefrontCommunity.Menu.Tests.Factories.OptionGroups;
using StorefrontCommunity.Menu.Tests.Factories.Options;
using StorefrontCommunity.Menu.Tests.Fakes;
using Xunit;

namespace StorefrontCommunity.Menu.Tests.Functional.Options
{
    public sealed class UpdateOptionTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public UpdateOptionTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldUpdateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup1 = new OptionGroup().To(itemGroup);
            var optionGroup2 = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup1);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.AddRange(optionGroup1, optionGroup2);
            _server.Database.Options.Add(option);

            await _server.Database.SaveChangesAsync();

            var path = $"/options/{option.Id}";
            var jsonRequest = new SaveOptionJson().Build(groupId: optionGroup2.Id);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<OptionJson>(response);

            await _server.Database.Entry(option).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_token.TenantId, option.TenantId);
            Assert.Equal(jsonRequest.GroupId, option.OptionGroupId);
            Assert.Equal(jsonRequest.Name, option.Name);
            Assert.Equal(jsonRequest.Description, option.Description);
            Assert.Equal(jsonRequest.Price, option.Price);
            Assert.Equal(jsonRequest.IsAvailable, option.IsAvailable);
            Assert.Equal(option.Id, jsonResponse.Id);
            Assert.Equal(option.OptionGroupId, optionGroup2.Id);
            Assert.Equal(option.Name, jsonResponse.Name);
            Assert.Equal(option.Description, jsonResponse.Description);
            Assert.Equal(option.Price, jsonResponse.Price);
            Assert.Equal(option.IsAvailable, jsonResponse.Available);
        }

        [Fact]
        public async Task ShouldPublishEventAfterUpdateSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup1 = new OptionGroup().To(itemGroup);
            var optionGroup2 = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup1);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.AddRange(optionGroup1, optionGroup2);
            _server.Database.Options.Add(option);

            await _server.Database.SaveChangesAsync();

            var path = $"options/{option.Id}";
            var jsonRequest = new SaveOptionJson().Build(groupId: optionGroup2.Id);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.option.updated");
            var payload = (OptionPayload)publishedEvent.Payload;

            await _server.Database.Entry(option).ReloadAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Id, option.Id);
            Assert.Equal(payload.TenantId, option.TenantId);
            Assert.Equal(payload.Name, option.Name);
            Assert.Equal(payload.Description, option.Description);
            Assert.Equal(payload.Price, option.Price);
            Assert.Equal(payload.IsAvailable, option.IsAvailable);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);

            await _server.Database.SaveChangesAsync();

            var path = "/options/5";
            var jsonRequest = new SaveOptionJson().Build(groupId: optionGroup.Id);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_NOT_FOUND", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentGroupId()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.Add(option);

            await _server.Database.SaveChangesAsync();

            var path = $"/options/{option.Id}";
            var jsonRequest = new SaveOptionJson().Build(groupId: 80);
            var response = await _client.PutJsonAsync(path, jsonRequest);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_GROUP_NOT_FOUND", jsonResponse.Error);
        }
    }
}
