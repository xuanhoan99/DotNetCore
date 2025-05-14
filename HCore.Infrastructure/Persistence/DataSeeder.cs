using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Data;
using System.Reflection;
using System.Security;
using System.Security.Claims;

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
            Log.Information("Đang chạy Seed Data...");
            // Đảm bảo database đã được tạo
            await context.Database.MigrateAsync();

            await EnsureSysMenuSeedAsync(context);

            // Thêm các role mặc định
            await EnsureRoleExists(roleManager, "Admin");

            // Thêm tài khoản Admin mặc định
            await EnsureAdminExists(userManager);
            Log.Information("Seed Data hoàn tất!");
        }

        private static async Task EnsureSysMenuSeedAsync(AppDbContext context)
        {
            if (await context.SysMenus.AnyAsync())
                return;

            var now = DateTime.UtcNow;

            var menus = new List<SysMenu>
            {
                new SysMenu
                {
                    Name = "Trang chủ",
                    EnglishName = "Home",
                    Url = "/app/Dashboard",
                    Order = 1,
                    ParentId = null,
                    Icon = "home",
                    CreatedBy = "system",
                    CreatedAt = now,
                    ApprovedBy = "admin",
                    ApprovedAt = now,
                    ApprovalStatus = "Approved",
                    IsDeleted = false
                },
                new SysMenu
                {
                    Name = "Quản trị hệ thống",
                    EnglishName = "SystemManager",
                    Url = "/app/admin/SystemManager",
                    Order = 2,
                    ParentId = null,
                    Icon = "fa-solid fa-folder",
                    CreatedBy = "system",
                    CreatedAt = now,
                    ApprovedBy = "admin",
                    ApprovedAt = now,
                    ApprovalStatus = "Approved",
                    IsDeleted = false
                },
                new SysMenu
                {
                    Name = "Quản lý người dùng",
                    EnglishName = "User",
                    Url = "/app/admin/Users",
                    Order = 1,
                    ParentId = 2,
                    Icon = "users",
                    CreatedBy = "system",
                    CreatedAt = now,
                    ApprovedBy = "admin",
                    ApprovedAt = now,
                    ApprovalStatus = "Approved",
                    IsDeleted = false
                },
                new SysMenu
                {
                    Name = "Thông tin trang",
                    EnglishName = "SysMenu",
                    Url = "/app/admin/SysMenu",
                    Order = 2,
                    ParentId = 2,
                    Icon = "fa-solid fa-folder",
                    CreatedBy = "system",
                    CreatedAt = now,
                    ApprovedBy = "admin",
                    ApprovedAt = now,
                    ApprovalStatus = "Approved",
                    IsDeleted = false
                },
                new SysMenu
                {
                    Name = "Vai trò",
                    EnglishName = "Role",
                    Url = "/app/admin/Roles",
                    Order = 3,
                    ParentId = 2, 
                    Icon = "shield",
                    CreatedBy = "system",
                    CreatedAt = now,
                    ApprovedBy = "admin",
                    ApprovedAt = now,
                    ApprovalStatus = "Approved",
                    IsDeleted = false
                }
            };            

            await context.SysMenus.AddRangeAsync(menus);
            await context.SaveChangesAsync();
        }

        private static async Task EnsureRoleExists(RoleManager<Role> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new Role(roleName);
                await roleManager.CreateAsync(role);
            }

            // Thêm các claim nếu chưa tồn tại
            var existingClaims = await roleManager.GetClaimsAsync(role);
            var claims = new[]
            {
                "Pages.Main.Home",
                "Pages.Main.SystemManager",
                "Pages.Main.User.Create",
                "Pages.Main.User.Update",
                "Pages.Main.User.Delete",
                "Pages.Main.User.View",
                "Pages.Main.User.Search",
                "Pages.Main.User.Reject",
                "Pages.Main.SysMenu.Create",
                "Pages.Main.SysMenu.Update",
                "Pages.Main.SysMenu.Delete",
                "Pages.Main.SysMenu.View",
                "Pages.Main.SysMenu.Search",
                "Pages.Main.SysMenu.Reject",
                "Pages.Main.Role.Create",
                "Pages.Main.Role.Update",
                "Pages.Main.Role.Delete",
                "Pages.Main.Role.View",
                "Pages.Main.Role.Search",
                "Pages.Main.Role.Reject"
            };

            foreach (var claimValue in claims)
            {
                if (!existingClaims.Any(c => c.Type == "permission" && c.Value == claimValue))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", claimValue));
                }
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
