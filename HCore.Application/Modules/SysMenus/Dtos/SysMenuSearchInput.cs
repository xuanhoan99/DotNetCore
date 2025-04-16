namespace HCore.Application.Modules.SysMenus.Dtos
{
    public class SysMenuSearchInput
    {
        public string? Name { get; set; }
        public string? EnglishName { get; set; }
        public int? ParentId { get; set; }

        // Phân trang
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
