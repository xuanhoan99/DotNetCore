using HCore.Application.Modules.Auth.Services;
using HCore.Application.Modules.Common.Constants;
using HCore.Application.Modules.Common.Responses;
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
        public async Task<BaseResponse<SysMenuDto>> Create([FromBody] SysMenuDto sysMenuDto)
        {
            var result = await _sysMenuService.Create(sysMenuDto);
            return result;
        }
        [HttpGet("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.View)]
        public async Task<BaseResponse<SysMenuDto>> GetById(int id)
        {
            var result = await _sysMenuService.GetById(id);
            return result;
        }
        [HttpPut("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Update)]
        public async Task<BaseResponse<SysMenuDto>> Update(int id, [FromBody] SysMenuDto sysMenuDto)
        {
            var result = await _sysMenuService.Update(id, sysMenuDto);
            return result;
        }

        [HttpDelete("{id}")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Delete)]
        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var result = await _sysMenuService.Delete(id);
            return result;
        }
        [HttpGet("all")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Search)]
        public async Task<BaseResponse<List<SysMenuDto>>> GetAll()
        {
            var result = await _sysMenuService.GetAll();
            return result;
        }
        [HttpPost("search")]
        [HCoreAuthorize(HCorePermissions.Prefix.Main, HCorePermissions.Page.SysMenu, HCorePermissions.Action.Search)]
        public async Task<PagedResponse<SysMenuDto>> Search([FromBody] SysMenuSearchInput input)
        {
            var result = await _sysMenuService.Search(input);
            return result;
        }
    }
}
