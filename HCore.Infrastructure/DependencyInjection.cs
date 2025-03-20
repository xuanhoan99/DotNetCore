

using HCore.Domain;
using HCore.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HCore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký ASP.NET Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Cấu hình Identity nếu cần
                options.Password.RequiredLength = 6;
                // ...
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
