using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Auth.Interfaces;
using HCore.Application.Modules.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<BaseResponse<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            return result;
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<BaseResponse<GetCurrentLoginInformationsOutput>> GetUserConfiguration()
        {
            var result = await _authService.GetUserConfigurationAsync();
            return result;
        }

        [HttpPost("refresh-token")]
        public async Task<BaseResponse<AuthResponseDto>> RefreshToken()
        {
            var result = await _authService.RefreshTokenAsync();
            return result;
        }

        [HttpPost("logout")]
        public async Task<BaseResponse<object>> Logout()
        {
            var result = await _authService.RevokeTokenAsync();
            return result;
        }

        [HttpPost("refresh-permissions/{userId}")]
        public async Task<BaseResponse<object>> ClearPermissionCacheAsync(string userId)
        {
            var result = await _authService.ClearPermissionCacheAsync(userId);
            return result;
        }
    }
}
