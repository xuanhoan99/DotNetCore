using AutoMapper;
using HCore.Application.Modules.Common.Responses;
using HCore.Application.Modules.SysMenus.Dtos;
using HCore.Application.Modules.SysMenus.Interfaces;
using HCore.Domain.Entities;
using HCore.Domain.Repositories;
using System.Linq.Expressions;

namespace HCore.Application.Modules.SysMenus.Service
{
    public class SysMenuService : ISysMenuService
    {
        private readonly IGenericRepository<SysMenu, int> _sysMenuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SysMenuService(
            IGenericRepository<SysMenu, int> sysMenuRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _sysMenuRepository = sysMenuRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<SysMenuDto>> Create(SysMenuDto input)
        {
            // 1. Kiểm tra trùng EnglishName
            var exists = await _sysMenuRepository.AnyAsync(x => x.EnglishName == input.EnglishName);
            if (exists)
            {
                return BaseResponse<SysMenuDto>.Fail("Menu English Name already exists.");
            }

            var entity = _mapper.Map<SysMenu>(input);
            await _sysMenuRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<SysMenuDto>(entity);
            return BaseResponse<SysMenuDto>.Ok(result, "Create successfully");
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var menu = await _sysMenuRepository.GetByIdAsync(id);
            if (menu == null)
                return BaseResponse<bool>.Fail("Menu not found");
            _sysMenuRepository.Delete(menu);
            await _unitOfWork.SaveChangesAsync();
            return BaseResponse<bool>.Ok(true, "Delete successfully");
        }

        public async Task<BaseResponse<List<SysMenuDto>>> GetAll()
        {
            try
            {
                var menus = await _sysMenuRepository.GetAllAsync(true);
                var result = _mapper.Map<List<SysMenuDto>>(menus);
                return BaseResponse<List<SysMenuDto>>.Ok(result, "Lấy danh sách menu thành công");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SysMenuDto>>.Fail("Đã xảy ra lỗi khi lấy danh sách menu: " + ex.Message);
            }
        }

        public async Task<BaseResponse<SysMenuDto>> GetById(int id)
        {
            try
            {
                var sysMenu = await _sysMenuRepository.GetByIdAsync(id);
                if (sysMenu == null)
                {
                    return BaseResponse<SysMenuDto>.Fail($"Không tìm thấy menu với ID = {id}");
                }

                var result = _mapper.Map<SysMenuDto>(sysMenu);
                return BaseResponse<SysMenuDto>.Ok(result, "Lấy chi tiết menu thành công");
            }
            catch (Exception ex)
            {
                return BaseResponse<SysMenuDto>.Fail("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        public async Task<PagedResponse<SysMenuDto>> Search(SysMenuSearchInput input)
        {
            Expression<Func<SysMenu, bool>> filter = u =>
                    (string.IsNullOrEmpty(input.Name) || u.Name.Contains(input.Name)) &&
                    (string.IsNullOrEmpty(input.EnglishName) || u.EnglishName.Contains(input.EnglishName)) &&
                    (!input.ParentId.HasValue || u.ParentId == input.ParentId);


            var result = await _sysMenuRepository.GetPagedAsync(
                    skip: (input.PageIndex - 1) * input.PageSize,
                    take: input.PageSize,
                    filter: filter,
                    orderBy: q => q.OrderBy(u => u.Name)
                );
            var data = _mapper.Map<List<SysMenuDto>>(result.Items);

            var response = PagedResponse<SysMenuDto>.Create(
                    data: data,
                    total: result.TotalItems,
                    pageIndex: input.PageIndex,
                    pageSize: input.PageSize
                );

            response.Message = "Lấy dữ liệu thành công";
            return response;
        }

        public async Task<BaseResponse<SysMenuDto>> Update(int id, SysMenuDto input)
        {
            try
            {
                var sysMenu = await _sysMenuRepository.GetByIdAsync(id);
                if (sysMenu == null)
                {
                    return BaseResponse<SysMenuDto>.Fail($"Không tìm thấy menu với ID = {id}");
                }
                // Cập nhật thuộc tính
                _mapper.Map(input, sysMenu);

                _sysMenuRepository.Update(sysMenu);
                await _unitOfWork.SaveChangesAsync();

                var updatedDto = _mapper.Map<SysMenuDto>(sysMenu);
                return BaseResponse<SysMenuDto>.Ok(updatedDto, "Cập nhật menu thành công");
            }
            catch (Exception ex)
            {
                return BaseResponse<SysMenuDto>.Fail("Lỗi khi cập nhật menu: " + ex.Message);
            }
        }
    }
}
