using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Users.UseCases
{
    public class CreateUser
    {
        private readonly IUserService _userService;

        public CreateUser(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> Execute(CreateUserDto dto)
        {
            return await _userService.CreateUserAsync(dto);
        }
    }

}
