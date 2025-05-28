using HCore.Application.Modules.Auth.Services;
using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.Create)]
        public async Task<BaseResponse<UserResponseDto>> Create([FromBody] UserDto userDto)
        {
            var result = await _userService.Create(userDto);
            return result;
        }

        [HttpGet("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.View)]
        public async Task<BaseResponse<UserResponseDto>> GetById(string id)
        {
            var result = await _userService.GetById(id);
            return result;
        }
        [HttpPut("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.Update)]
        public async Task<BaseResponse<UserResponseDto>> Update(string id, [FromBody] UserDto user)
        {
            var result = await _userService.Update(id, user);
            return result;
        }
        [HttpDelete("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.Delete)]
        public async Task<BaseResponse<bool>> Delete(string id)
        {
            var result = await _userService.Delete(id);
            return result;
        }
        [HttpGet("all")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.Search)]
        public async Task<BaseResponse<List<UserResponseDto>>> GetAll()
        {
            var result = await _userService.GetAllUser();
            return result;
        }
    }
}
