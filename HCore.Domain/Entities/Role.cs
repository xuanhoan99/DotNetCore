using Microsoft.AspNetCore.Identity;

namespace HCore.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public string? Desc { get; set; }
        public Role() : base() { }
        public Role(string roleName) : base(roleName) { }
    }
}
