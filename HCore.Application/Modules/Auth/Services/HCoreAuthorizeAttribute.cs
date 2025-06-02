using Microsoft.AspNetCore.Authorization;

namespace HCore.Application.Modules.Auth.Services
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class HCoreAuthorizeAttribute : AuthorizeAttribute
    {
        public HCoreAuthorizeAttribute(string module, string page, string action)
        {
            Policy = $"{module}.{page}.{action}";
        }
    }
}
