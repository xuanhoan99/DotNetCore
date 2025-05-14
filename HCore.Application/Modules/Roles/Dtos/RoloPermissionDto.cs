namespace HCore.Application.Modules.Roles.Dtos
{
    public class RoloPermissionDto
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsDto> RoleClaims { get; set; }
    }
    public class RoleClaimsDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
