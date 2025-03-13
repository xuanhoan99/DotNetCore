using HCore.Domain.Common;

namespace HCore.Domain.Entities
{
    public class Role : AuditEntityBase<Guid>
    {
        public string Name { get; private set; }

        public Role(Guid id, string name, string createdBy) : base(id, createdBy)
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
