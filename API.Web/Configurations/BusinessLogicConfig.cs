using API.BusinessLogic;
using API.Common;
using API.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace API.WEB
{
    public static class BusinessLogicConfig
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceBusinessLogic, ServiceBusinessLogic>()
                    .AddScoped<IStoredProcedures, StoredProcedures>()
                    .AddHostedService<AppInitializationService>()
                    .AddScoped(typeof(IServiceLogger<>), typeof(ServiceLogger<>));
            return services;
        }
    }
}
