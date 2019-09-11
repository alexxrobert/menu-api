using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Storefront.Menu.API.Authorization;
using Storefront.Menu.API.Models.DataModel;


namespace Storefront.Menu.Tests.Fakes
{
    public sealed class ApiServer : TestServer
    {
        public ApiServer() : base(new Program().CreateWebHostBuilder()) { }

        public ApiDbContext Database => Host.Services.GetService<ApiDbContext>();
        public JwtOptions JwtOptions => Host.Services.GetService<IOptions<JwtOptions>>().Value;
    }
}
