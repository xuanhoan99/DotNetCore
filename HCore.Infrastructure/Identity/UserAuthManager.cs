using HCore.Application.Modules.Auth.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HCore.Infrastructure.Identity
{
    public class UserAuthManager : IUserAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserAuthManager(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public string? UserName =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public async Task<User?> FindByUsernameAsync(string userName)
            => await _userManager.FindByNameAsync(userName);

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };
            // Thêm các roles của user vào claims
            //var roles = await _userManager.GetRolesAsync(user);
            //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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

        public async Task<User?> GetCurrentUserAsync()
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(id) ? null : await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
            => await _userManager.GetRolesAsync(user);

        public async Task<Role?> FindByRolenameAsync(string roleName)
            => await _roleManager.FindByNameAsync(roleName);

        public async Task<IList<Claim?>> GetClaimsByRoleAsync(Role role)
            => await _roleManager.GetClaimsAsync(role);
    }

}
