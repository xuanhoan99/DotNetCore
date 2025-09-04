namespace HCore.Application.Modules.Users.Dtos
{
    public class UserSearchInput
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }

        // Phân trang
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
