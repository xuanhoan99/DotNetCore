using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HCore.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            Log.Information("🔄 Đang chạy Seed Data...");
            // Đảm bảo database đã được tạo
            await context.Database.MigrateAsync();

            // Thêm các role mặc định
            await EnsureRoleExists(roleManager, "Admin");
            await EnsureRoleExists(roleManager, "Member");

            // Thêm tài khoản Admin mặc định
            await EnsureAdminExists(userManager);
            Log.Information("🎉 Seed Data hoàn tất!");
        }

        private static async Task EnsureRoleExists(RoleManager<Role> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role(roleName));
            }
        }

        private static async Task EnsureAdminExists(UserManager<User> userManager)
        {
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123";
            string userName = "admin";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = userName,
                    Name = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
