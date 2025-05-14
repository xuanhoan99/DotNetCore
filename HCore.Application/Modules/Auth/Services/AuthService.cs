using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Auth.Interfaces;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HCore.Application.Modules.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return BaseResponse<AuthResponseDto>.Fail("Invalid username or password");
            }

            var login = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!login.Succeeded)
            {
                return BaseResponse<AuthResponseDto>.Fail("Invalid username or password");
            }

            var token = await GenerateToken(user);
            var result = new AuthResponseDto
            {
                Token = token
            };
            return BaseResponse<AuthResponseDto>.Ok(result, "Login successful");
        }

        private async Task<string> GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            // Thêm các roles của user vào claims
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
