using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserQueries _userQueries;

        public UsersController(IUserService userService, IUserQueries userQueries)
        {
            _userService = userService;
            _userQueries = userQueries;
        }
        [HttpPost]
        [HCoreAuthorize(HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.Create)]
        public async Task<BaseResponse<UserResponseDto>> Create([FromBody] UserDto userDto)
        {
            var result = await _userService.Create(userDto);
            return result;
        }

        [HttpGet("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.View)]
        public async Task<BaseResponse<UserResponseDto>> GetById(string id)
        {
            var result = await _userService.GetById(id);
            return result;
        }
        [HttpPut("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.Update)]
        public async Task<BaseResponse<UserResponseDto>> Update(string id, [FromBody] UserDto user)
        {
            var result = await _userService.Update(id, user);
            return result;
        }
        [HttpDelete("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.Delete)]
        public async Task<BaseResponse<bool>> Delete(string id)
        {
            var result = await _userService.Delete(id);
            return result;
        }
        [HttpGet("all")]
        [HCoreAuthorize(HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.Search)]
        public async Task<BaseResponse<List<UserResponseDto>>> GetAll()
        {
            var result = await _userService.GetAllUser();
            return result;
        }
        [HttpPost("search")]
        [HCoreAuthorize(HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.Search)]
        public async Task<PagedResponse<UserSearchResponseDto>> Search([FromBody] UserSearchInput input)
        {
            var result = await _userQueries.SearchAsync(input);
            return result;
        }
        [HttpGet("{id}/roles")]
        [HCoreAuthorize( HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.Search)]
        public async Task<BaseResponse<List<string>>> GetUserRoles(string id)
        {
            return await _userService.GetUserRoles(id);
        }

        [HttpPost("{id}/roles")]
        [HCoreAuthorize(
            HCorePermissions.Prefix.Administration, HCorePermissions.Page.User, HCorePermissions.Action.Update)]
        public async Task<BaseResponse<bool>> AssignRoles(string id, [FromBody] List<string> roleNames)
        {
            return await _userService.AssignRoles(id, roleNames);
        }
    }
}
