using HCore.Application.Modules.Auth.Interfaces;
using HCore.Application.Modules.Auth.Services;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Application.Modules.Roles.Services;
using HCore.Application.Modules.SysMenus.Interfaces;
using HCore.Application.Modules.SysMenus.Service;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Application.Modules.Users.Services;
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISysMenuService, SysMenuService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }
    }
}
