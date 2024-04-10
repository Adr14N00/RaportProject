using RaportAPI.Core.Application.Common.Interfaces;
using RaportAPI.Infrastructure.Infrastructure.Services;

namespace RaportAPI.Infrastructure.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IExportsHistoryService, ExportsHistoryService>();
            return services;
        }
    }
}
