using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Storefront.Menu.API.Authorization;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.Tests.Fakes
{
    public sealed class FakeApiServer : TestServer
    {
        public FakeApiServer() : base(new Program().CreateWebHostBuilder()) { }

        public ApiDbContext Database => Host.Services.GetService<ApiDbContext>();
        public FakeMessageBroker EventBus => Host.Services.GetService<IMessageBroker>() as FakeMessageBroker;
        public JwtOptions JwtOptions => Host.Services.GetService<IOptions<JwtOptions>>().Value;
    }
}
