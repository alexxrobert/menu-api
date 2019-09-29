using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorefrontCommunity.Menu.API.Authorization;
using StorefrontCommunity.Menu.API.Filters;
using StorefrontCommunity.Menu.API.Models.DataModel;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;
using StorefrontCommunity.Menu.Tests.Fakes;

namespace StorefrontCommunity.Menu.Tests
{
    public sealed class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequestValidationFilter());
            })
            .AddApplicationPart(typeof(StorefrontCommunity.Menu.API.Startup).Assembly);

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseInMemoryDatabase("db_test");
            });

            services.AddDefaultCorsPolicy();
            services.AddJwtAuthentication(_configuration.GetSection("Auth"));

            services.AddSingleton<EventBinding>();

            services.AddSingleton<IMessageBroker, FakeMessageBroker>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
