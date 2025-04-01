using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCore.Application.Modules.Users.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserResponseDto?> Create(UserDto input)
        {
            var user = new User()
            {
                Name = input.Name,
                Email = input.Email,
                UserName = input.UserName,
            };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                return null; // Trả về null nếu tạo thất bại
            }

            var userResponse = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email
            };

            return userResponse;
        }

        public async Task<IdentityResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<UserResponseDto>> GetAllUser()
        {
            var users = await _userManager.Users.Select(s => new UserResponseDto()
            {
                Id = s.Id,
                Name = s.Name,
                UserName = s.UserName,
                Email = s.Email
            }).ToListAsync();
            return users;
        }

        public async Task<UserResponseDto> GetById(string id)
        {
            UserResponseDto userResponseDto;
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                userResponseDto = new UserResponseDto();
            }
            else
            {
                userResponseDto = new UserResponseDto()
                {
                    Id = user.Id,
                    Name = user.Name
                };
            }
            return userResponseDto;
        }

        public async Task<IdentityResult> Update(string id, UserDto input)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Name = input.Name;
            return await _userManager.UpdateAsync(user);
        }
    }
}
