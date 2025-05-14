using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Users.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HCore.Application.Modules.Users.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponseDto>> Create(UserDto input);
        Task<BaseResponse<bool>> Delete(string id);
        Task<BaseResponse<List<UserResponseDto>>> GetAllUser();
        Task<BaseResponse<UserResponseDto>> GetById(string id);
        Task<BaseResponse<UserResponseDto>> Update(string id, UserDto input);
        string? UserName { get; }
    }
}
