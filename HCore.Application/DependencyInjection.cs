using HCore.Application.Modules.Roles.Interfaces;
using HCore.Application.Modules.Roles.Services;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Application.Modules.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HCore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();

            // Đăng ký AutoMapper 
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
