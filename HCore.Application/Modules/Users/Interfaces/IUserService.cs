using HCore.Application.Modules.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Users.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(Guid id);
    }

}
