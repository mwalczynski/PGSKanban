using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PgsKanban.Hubs.Hubs;

namespace PgsKanban.Api.Config
{
    public static class SignalRConfiguration
    {
        public const string ROUTE_PREFIX = "hubs";
        public static void AddSignalR(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public static void UseSignalR(IApplicationBuilder app)
        {
            app.UseSignalR(routes =>
            {
                routes.MapHub<SampleHub>($"{ROUTE_PREFIX}/sample");
                routes.MapHub<CardDetailsHub>($"{ROUTE_PREFIX}/details");
            });
        }
    }
}
