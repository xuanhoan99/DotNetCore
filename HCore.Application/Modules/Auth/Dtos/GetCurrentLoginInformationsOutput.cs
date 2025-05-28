namespace HCore.Application.Modules.Auth.Dtos
{
    public class GetCurrentLoginInformationsOutput
    {
        public CurrentUserDto User { get; set; }
        public List<string> Permissions { get; set; }
    }
    public class CurrentUserDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
