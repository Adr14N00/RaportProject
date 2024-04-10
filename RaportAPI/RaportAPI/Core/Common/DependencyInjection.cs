using RaportAPI.Core.Common.Middlewares;

namespace RaportAPI.Core.Common
{
    public static class DependencyInjection
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}
