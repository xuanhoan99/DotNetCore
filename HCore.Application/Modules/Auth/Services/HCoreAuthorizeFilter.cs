using HCore.Application.Modules.Auth.Interfaces;
using HCore.Application.Modules.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HCore.Application.Modules.Auth.Services
{
    public class HCoreAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthService _authService;


        public HCoreAuthorizeFilter(
            IAuthService authService)
        {
            _authService = authService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attribute = descriptor?.MethodInfo
                .GetCustomAttributes(typeof(HCoreAuthorizeAttribute), true)
                .FirstOrDefault() as HCoreAuthorizeAttribute;

            if (attribute != null)
            {
                var userPermissions = await _authService.GetAllPermissionsForCurrentUserAsync();
                if (!attribute.Permissions.Any(p => userPermissions.Contains(p)))
                {
                    context.Result = new JsonResult(BaseResponse<string>.Fail("Permission denied"))
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                }
            }
        }
    }

}
