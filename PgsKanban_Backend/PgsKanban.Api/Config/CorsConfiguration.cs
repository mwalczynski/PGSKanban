using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PgsKanban.Api.Config
{
    public class CorsConfiguration
    {
        public static void AddCors(IServiceCollection services)
        {
            services.AddCors();
        }
        public static void UseCors(IApplicationBuilder app)
        {
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        }
    }
}
