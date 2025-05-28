using HCore.Domain.Common;

namespace HCore.Domain.Entities
{
    public class SysMenu : AuditEntityBase<int>
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Url { get; set; }
        public int? Order { get; set; }
        public int? ParentId { get; set; }
        public string? Icon { get; set; }
        public string PermissionName { get; set; }
    }
}
