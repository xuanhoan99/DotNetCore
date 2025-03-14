using HCore.Domain.Common;

namespace HCore.Domain.Entities
{
    public class Role : AuditEntityBase
    {
        public int Id { get; set; }
        public string Name { get; private set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public Role(string name, string createdBy) : base( createdBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be empty.");

            Name = name;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be empty.");
            Name = name;
            //UpdateTimestamp();
        }
    }
}
