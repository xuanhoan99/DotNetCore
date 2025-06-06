using Microsoft.AspNetCore.Authorization;

namespace HCore.Infrastructure.Authorization
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
