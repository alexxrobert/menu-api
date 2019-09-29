using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.EventModel.Published.Options;
using StorefrontCommunity.Menu.API.Models.TransferModel;
using StorefrontCommunity.Menu.Tests.Factories.ItemGroups;
using StorefrontCommunity.Menu.Tests.Factories.OptionGroups;
using StorefrontCommunity.Menu.Tests.Factories.Options;
using StorefrontCommunity.Menu.Tests.Fakes;
using Xunit;

namespace StorefrontCommunity.Menu.Tests.Functional.Options
{
    public sealed class DeleteOptionTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiToken _token;
        private readonly FakeApiClient _client;

        public DeleteOptionTest()
        {
            _server = new FakeApiServer();
            _token = new FakeApiToken(_server.JwtOptions);
            _client = new FakeApiClient(_server, _token);
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.Add(option);

            await _server.Database.SaveChangesAsync();

            var path = $"/options/{option.Id}";
            var response = await _client.DeleteAsync(path);
            var hasBeenDeleted = !await _server.Database.Options
                .WhereKey(_token.TenantId, option.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task ShouldPublishEventAfterDeleteSuccessfully()
        {
            var itemGroup = new ItemGroup().Of(_token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);
            var option = new Option().To(optionGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            _server.Database.Options.Add(option);

            await _server.Database.SaveChangesAsync();

            var path = $"/options/{option.Id}";
            var response = await _client.DeleteAsync(path);
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.option.deleted");
            var payload = (OptionPayload)publishedEvent.Payload;

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
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
            var path = "/options/5";
            var response = await _client.DeleteAsync(path);
            var jsonResponse = await _client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_NOT_FOUND", jsonResponse.Error);
        }
    }
}
