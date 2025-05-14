using HCore.Application.Modules.Auth.Services;
using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            var result = await _userService.Create(userDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpGet("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.View)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _userService.GetById(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpPut("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.Update)]
        public async Task<IActionResult> Update(string id, [FromBody] UserDto user)
        {
            var result = await _userService.Update(id, user);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpDelete("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.Delete(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("all")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.User, HCorePermissions.Action.Search)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUser();
            return Ok(result);
        }
    }
}
