using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PgsKanban.Api.Config;
using PgsKanban.BusinessLogic.Config;
using PgsKanban.DataAccess;

namespace PgsKanban.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDbContext<PgsKanbanContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            IdentityConfiguration.AddIdentity(services);
            SwaggerConfiguration.ConfigureSwagger(services);
            SignalRConfiguration.AddSignalR(services);
            MvcConfiguration.AddMvc(services);
            IdentityConfiguration.ConfigureIdentity(services);
            CorsConfiguration.AddCors(services);
            OptionsRegistration.Configure(Configuration, services);
            services.AddSingleton<IMapper>(sp => AutoMapperConfig.Initialize());
            JwtConfiguration.AddJwtAuthorization(Configuration, services);

            return DependencyRegistration.Register(Configuration, services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            LoggingConfiguration.ConfigureLogging(app, env, loggerFactory, Configuration);
            app.UseAuthentication();
            SwaggerConfiguration.AddSwagger(app);
            CorsConfiguration.UseCors(app);
            if (!env.IsDevelopment())
            {
                var context = app.ApplicationServices.GetService<PgsKanbanContext>();
                context.Database.Migrate();
            }
 
            SignalRConfiguration.UseSignalR(app);
            app.UseMvc();
        }
    }
}
