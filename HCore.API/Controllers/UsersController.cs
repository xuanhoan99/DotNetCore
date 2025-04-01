using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers
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
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            var user = await _userService.Create(userDto);

            if (user == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { id = user!.Id }, user);

        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _userService.GetById(id);
            if (result == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserDto user)
        {
            var result = await _userService.Update(id, user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.Delete(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUser();
            return Ok(result);
        }
    }
}
