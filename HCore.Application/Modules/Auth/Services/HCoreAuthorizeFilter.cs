using HCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Auth.Services
{
    public class HCoreAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public HCoreAuthorizeFilter(IHttpContextAccessor httpContextAccessor,RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attribute = descriptor?.MethodInfo
                .GetCustomAttributes(typeof(HCoreAuthorizeAttribute), true)
                .FirstOrDefault() as HCoreAuthorizeAttribute;

            if (attribute != null)
            {
                var userPermissions = await GetAllClaimsForUserAsync();
                if (!attribute.Permissions.Any(p => userPermissions.Contains(p)))
                {
                    context.Result = new ForbidResult("Permission denied");
                }
            }
        }

        private async Task<List<string>> GetAllClaimsForUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return new List<string>();

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new List<string>();

            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null) return new List<string>();


            var roleNames = await _userManager.GetRolesAsync(appUser);
            var roleClaims = new List<Claim>();

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    roleClaims.AddRange(claims);
                }
            }
            var permissions = roleClaims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .Distinct()
                .ToList();

            return permissions;
        }
    }
    
}
