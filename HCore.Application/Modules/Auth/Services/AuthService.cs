using AutoMapper;
using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Auth.Interfaces;
using HCore.Application.Modules.Common.Responses;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HCore.Application.Modules.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserAuthManager _authManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public AuthService(
            IUserAuthManager authManager,
            IMapper mapper,
            IConfiguration configuration,
            IRefreshTokenService refreshTokenService,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache cache)
        {
            _authManager = authManager;
            _mapper = mapper;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _authManager.FindByUsernameAsync(request.UserName);
            if (user == null || !await _authManager.CheckPasswordAsync(user, request.Password))
            {
                return BaseResponse<AuthResponseDto>.Fail("Invalid username or password");
            }

            await _refreshTokenService.RevokeAllUserTokensAsync(user.Id);

            var token = this.GenerateToken(user);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);


            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refresh_token", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Expires = refreshToken.ExpiryDate
            });

            var result = new AuthResponseDto
            {
                Token = token,
            };
            return BaseResponse<AuthResponseDto>.Ok(result, "Login successful");
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

        public async Task<BaseResponse<AuthResponseDto>> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return BaseResponse<AuthResponseDto>.Fail("Refresh token not found in cookie");

            var storedToken = await _refreshTokenService.GetByTokenAsync(refreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return BaseResponse<AuthResponseDto>.Fail("Refresh token is invalid or expired.");
            }

            var user = storedToken.User;
            if (user == null)
            {
                return BaseResponse<AuthResponseDto>.Fail("User not found.");
            }

            // Revoke old token
            await _refreshTokenService.RevokeTokenAsync(storedToken);

            // Generate new tokens
            var newAccessToken = GenerateToken(user);
            var newRefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refresh_token", newRefreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefreshToken.ExpiryDate
            });

            return BaseResponse<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = newAccessToken
            }, "Refresh token successful");
        }

        public async Task<BaseResponse<object>> RevokeTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return BaseResponse<object>.Fail("Refresh token not found in cookie");

            var storedToken = await _refreshTokenService.GetByTokenAsync(refreshToken);
            if (storedToken == null || storedToken.IsRevoked)
                return BaseResponse<object>.Fail("Invalid refresh token.");

            await _refreshTokenService.RevokeTokenAsync(storedToken);
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refresh_token");
            return BaseResponse<object>.Ok(null, "Token revoked successfully.");
        }

        public Task<BaseResponse<object>> ClearPermissionCacheAsync(string userId)
        {
            var cacheKey = $"perm:uid:{userId}";
            _cache.Remove(cacheKey);

            return Task.FromResult(BaseResponse<object>.Ok(null, "Permission cache cleared successfully."));
        }

        private async Task<List<string>> GetAllPermissionsForCurrentUserAsync()
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

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
