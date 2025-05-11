using Microsoft.AspNetCore.Identity;

namespace HCore.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User() : base() { }

        public string Name { get; set; }
    }
}
