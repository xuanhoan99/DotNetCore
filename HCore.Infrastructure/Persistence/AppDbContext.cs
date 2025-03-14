using HCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCore.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //user
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FullName).HasMaxLength(255);
            });

            //role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.HasKey(u => u.Id);
            });

            //userrole
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.Property(u => u.Id).ValueGeneratedOnAdd(); 
            });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }
    }
}
