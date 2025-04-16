using HCore.Application.Modules.Common;
using HCore.Application.Modules.SysMenus.Dtos;

namespace HCore.Application.Modules.SysMenus.Interfaces
{
    public interface ISysMenuService
    {
        Task<BaseResponse<SysMenuDto>> Create(SysMenuDto input);
        Task<BaseResponse<bool>> Delete(int id);
        Task<BaseResponse<List<SysMenuDto>>> GetAll();
        Task<BaseResponse<SysMenuDto>> GetById(int id);
        Task<BaseResponse<SysMenuDto>> Update(int id, SysMenuDto input);
        Task<PagedResponse<SysMenuDto>> Search(SysMenuSearchInput input);
    }
}
