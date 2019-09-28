using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.EventModel.Published.OptionGroups;
using Storefront.Menu.API.Models.TransferModel.Errors;
using Storefront.Menu.Tests.Factories.ItemGroups;
using Storefront.Menu.Tests.Factories.OptionGroups;
using Storefront.Menu.Tests.Factories.Options;
using Storefront.Menu.Tests.Fakes;
using Xunit;

namespace Storefront.Menu.Tests.Functional.OptionGroups
{
    public sealed class DeleteOptionGroupTest
    {
        private readonly FakeApiServer _server;

        public DeleteOptionGroupTest()
        {
            _server = new FakeApiServer();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}/option-groups/{optionGroup.Id}";
            var response = await client.DeleteAsync(path);
            var hasBeenDeleted = !await _server.Database.OptionGroups
                .WhereKey(token.TenantId, optionGroup.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task ShouldPublishEventAfterDeleteSuccessfully()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);
            var optionGroup = new OptionGroup().To(itemGroup);

            _server.Database.ItemGroups.Add(itemGroup);
            _server.Database.OptionGroups.Add(optionGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}/option-groups/{optionGroup.Id}";
            var response = await client.DeleteAsync(path);
            var publishedEvent = _server.EventBus.PublishedEvents
                .Single(@event => @event.Name == "menu.option-group.deleted");
            var payload = (OptionGroupPayload)publishedEvent.Payload;

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(payload.Id, optionGroup.Id);
            Assert.Equal(payload.TenantId, optionGroup.TenantId);
            Assert.Equal(payload.Title, optionGroup.Title);
        }

        [Fact]
        public async Task ShouldRespond422ForInexistentId()
        {
            var token = new FakeApiToken(_server.JwtOptions);
            var client = new FakeApiClient(_server, token);

            var itemGroup = new ItemGroup().Of(token.TenantId);

            _server.Database.ItemGroups.Add(itemGroup);
            await _server.Database.SaveChangesAsync();

            var path = $"/item-groups/{itemGroup.Id}/option-groups/5";
            var response = await client.DeleteAsync(path);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_GROUP_NOT_FOUND", jsonResponse.Error);
        }

        [Fact]
        public async Task ShouldRespond422IfGroupHasOptions()
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

            var path = $"/item-groups/{itemGroup.Id}/option-groups/{optionGroup.Id}";
            var response = await client.DeleteAsync(path);
            var jsonResponse = await client.ReadJsonAsync<UnprocessableEntityError>(response);
            var hasBeenDeleted = !await _server.Database.OptionGroups
                .WhereKey(token.TenantId, optionGroup.Id)
                .AnyAsync();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("OPTION_GROUP_HAS_OPTIONS", jsonResponse.Error);
            Assert.False(hasBeenDeleted);
        }
    }
}
