using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Auth.Interfaces;
using HCore.Application.Modules.Roles.Dtos;
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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
