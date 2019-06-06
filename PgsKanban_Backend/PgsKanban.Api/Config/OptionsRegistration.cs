using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PgsKanban.BusinessLogic.Options;

namespace PgsKanban.Api.Config
{
    public class OptionsRegistration
    {
        public static void Configure(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<CacheOptions>(configuration.GetSection("cache"));
            services.Configure<MessageSenderOptions>(configuration.GetSection("MailOptions"));
            services.Configure<OpenIdOptions>(configuration.GetSection("OpenId"));
            services.Configure<GoogleOptions>(configuration.GetSection("Google"));
            services.Configure<FacebookOptions>(configuration.GetSection("Facebook"));
            services.Configure<AvailableExternalProvidersOptions>(configuration.GetSection("ExternalLoginProviders"));
            services.Configure<IPGeoOptions>(configuration.GetSection("IPGeoAPI"));
        }
    }
}
