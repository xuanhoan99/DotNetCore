using AutoMapper;
using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Auth.Interfaces;
using HCore.Application.Modules.Common.Responses;
using System.Security.Claims;

namespace HCore.Application.Modules.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserAuthManager _authManager;
        private readonly IMapper _mapper;

        public AuthService(
            IUserAuthManager authManager,
            IMapper mapper)
        {
            _authManager = authManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _authManager.FindByUsernameAsync(request.UserName);
            if (user == null || !await _authManager.CheckPasswordAsync(user, request.Password))
            {
                return BaseResponse<AuthResponseDto>.Fail("Invalid username or password");
            }

            var token = await _authManager.GenerateTokenAsync(user);
            var result = new AuthResponseDto
            {
                Token = token
            };
            return BaseResponse<AuthResponseDto>.Ok(result, "Login successful");
        }

        public async Task<List<string>> GetAllPermissionsForCurrentUserAsync()
        {
            var user = await _authManager.GetCurrentUserAsync();
            if (user == null) return new List<string>();

            var roles = await _authManager.GetUserRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var roleName in roles)
            {
                var role = await _authManager.FindByRolenameAsync(roleName);
                if (role != null)
                {
                    var claims = await _authManager.GetClaimsByRoleAsync(role);
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

        public async Task<BaseResponse<GetCurrentLoginInformationsOutput>> GetUserConfigurationAsync()
        {
            var permissions = await GetAllPermissionsForCurrentUserAsync();

            var appUser = await _authManager.GetCurrentUserAsync();

            var userResult = _mapper.Map<CurrentUserDto>(appUser);

            var result = new GetCurrentLoginInformationsOutput
            {
                User = userResult,
                Permissions = permissions
            };

            return BaseResponse<GetCurrentLoginInformationsOutput>.Ok(result, "GetUserConfiguration successful");
        }
    }
}
