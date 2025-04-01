using Microsoft.AspNetCore.Identity;

namespace HCore.Domain.Entities
{
    public class User : IdentityUser
    {
        public User() : base() { }

        public string Name { get; set; }
    }
}
