using Microsoft.Extensions.DependencyInjection;
using PgsKanban.Api.Attributes;

namespace PgsKanban.Api.Config
{
    public class MvcConfiguration
    {
        public static void AddMvc(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidateModelStateAttribute());
            });
        }
    }
}
