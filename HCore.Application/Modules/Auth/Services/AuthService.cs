using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Auth.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            // 1️⃣ Kiểm tra user có tồn tại không
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return new AuthResponseDto { Message = "Invalid username or password" };
            }

            // 2️⃣ Kiểm tra password
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                return new AuthResponseDto { Message = "Invalid username or password" };
            }

            // 3️⃣ Sinh JWT Token
            var token = await GenerateToken(user);

            return new AuthResponseDto { Token = token, Message = "Login successful" };
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
