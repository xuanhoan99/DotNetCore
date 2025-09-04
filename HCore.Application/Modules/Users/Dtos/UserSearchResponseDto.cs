namespace HCore.Application.Modules.Users.Dtos
{
    public class UserSearchResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string UserName { get; set; } = default!;
        public string? Email { get; set; }
        public string Roles { get; set; } = string.Empty;
    }
}
