using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HCore.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);     
            
            builder.Entity<IdentityRole>().Property(x=>x.Id).HasMaxLength(50).IsUnicode(false);
            builder.Entity<User>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        }
    }
}
