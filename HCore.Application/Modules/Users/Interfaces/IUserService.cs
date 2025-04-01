using HCore.Application.Modules.Users.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HCore.Application.Modules.Users.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> Create(UserDto input);
        Task<IdentityResult> Delete(string id);
        Task<List<UserResponseDto>> GetAllUser();
        Task<UserResponseDto> GetById(string id);
        Task<IdentityResult> Update(string id, UserDto input);
    }
}
