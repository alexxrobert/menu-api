using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorefrontCommunity.Menu.API.Authorization;
using StorefrontCommunity.Menu.API.Filters;
using StorefrontCommunity.Menu.API.Models.DataModel;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus.RabbitMQ;
using StorefrontCommunity.Menu.API.Swagger;

namespace StorefrontCommunity.Menu.API
{
    [ExcludeFromCodeCoverage]
    public sealed class Startup
    {
        public IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMQOptions>(_configuration.GetSection("RabbitMQ"));

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseNpgsql(_configuration["ConnectionString:PostgreSQL"], pgsql =>
                {
                    pgsql.MigrationsHistoryTable(tableName: "__migration_history", schema: ApiDbContext.Schema);
                });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequestValidationFilter());
            });

            services.AddDefaultCorsPolicy();
            services.AddJwtAuthentication(_configuration.GetSection("Auth"));
            services.AddSwaggerDocumentation();

            services.AddScoped<IMessageBroker, RabbitMQBroker>();

            services.AddSingleton<EventBinding>();

            services.AddHostedService<RabbitMQBroker>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseSwaggerDocumentation();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
