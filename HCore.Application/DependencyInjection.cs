using Microsoft.Extensions.DependencyInjection;

namespace HCore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Đăng ký AutoMapper 
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
