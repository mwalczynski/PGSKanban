using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PgsKanban.DataAccess;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.Api.Config
{
    public class IdentityConfiguration
    {
        public static void ConfigureIdentity(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
        }

        public static void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PgsKanbanContext>()
                .AddDefaultTokenProviders();
        }
    }
}
