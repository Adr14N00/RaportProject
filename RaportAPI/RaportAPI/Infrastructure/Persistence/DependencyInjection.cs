using RaportAPI.Core.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RaportAPI.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RaportsDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("RaportsDB")));
            services.AddScoped<IRaportsDbContext, RaportsDbContext>();

            return services;

        }
    }
}
