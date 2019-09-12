using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storefront.Menu.API.Authorization;
using Storefront.Menu.API.Filters;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;
using Storefront.Menu.Tests.Fakes;

namespace Storefront.Menu.Tests
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
                options.Filters.Add(new RequestBodyValidationFilter());
            })
            .AddApplicationPart(typeof(Storefront.Menu.API.Startup).Assembly);

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseInMemoryDatabase("db_test");
            });

            services.AddDefaultCorsPolicy();
            services.AddJwtAuthentication(_configuration.GetSection("Auth"));

            services.AddSingleton<IEventBus, FakeEventBus>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
