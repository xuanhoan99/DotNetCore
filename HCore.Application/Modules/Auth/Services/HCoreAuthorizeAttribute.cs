namespace HCore.Application.Modules.Auth.Services
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HCoreAuthorizeAttribute : Attribute
    {
        public string[] Permissions { get; }
        public HCoreAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }
        public HCoreAuthorizeAttribute(string prefix, string pageName, string action)
        {
            Permissions = new[] { $"{prefix}.{pageName}.{action}" };
        }
    }
}
