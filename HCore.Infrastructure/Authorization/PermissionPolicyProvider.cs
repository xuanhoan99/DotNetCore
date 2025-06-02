using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HCore.Infrastructure.Authorization
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(
            IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Nếu policyName chứa dấu '.', là permission động
            if (policyName.Contains("."))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();
                return policy;
            }
            // policy mặc định
            return await base.GetPolicyAsync(policyName);
        }
    }
}
