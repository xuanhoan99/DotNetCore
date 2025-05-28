namespace HCore.Application.Modules.SysMenus.Dtos
{
    public class SysMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Url { get; set; }
        public int? Order { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsDeleted { get; set; }
        public string PermissionName { get; set; }
    }
}
