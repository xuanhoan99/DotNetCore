using HCore.Application.Modules.Roles.Interfaces;
using HCore.Application.Modules.Roles.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HCore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Đăng ký AutoMapper 
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }
    }
}
