using HCore.Application.Modules.Auth.Services;
using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.SysMenus.Dtos;
using HCore.Application.Modules.SysMenus.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HCore.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysMenusController : ControllerBase
    {
        private readonly ISysMenuService _sysMenuService;

        public SysMenusController(ISysMenuService userService)
        {
            _sysMenuService = userService;
        }
        [HttpPost]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Create)]
        public async Task<IActionResult> Create([FromBody] SysMenuDto sysMenuDto)
        {
            var result = await _sysMenuService.Create(sysMenuDto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);

        }
        [HttpGet("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.View)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _sysMenuService.GetById(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPut("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] SysMenuDto sysMenuDto)
        {
            var result = await _sysMenuService.Update(id, sysMenuDto);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sysMenuService.Delete(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpGet]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Search)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sysMenuService.GetAll();
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPost("search")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Search)]
        public async Task<IActionResult> Search([FromBody] SysMenuSearchInput input)
        {
            var result = await _sysMenuService.Search(input);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
