using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HCore.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<SysMenu> SysMenus { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Todo Cấu hình bảng trung gian giữa User và Role
            builder.Entity<IdentityUserRole<int>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");


        }
    }
}
