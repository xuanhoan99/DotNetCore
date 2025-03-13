using AutoMapper;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Domain;
using HCore.Domain.Entities;
using HCore.Domain.Repositories;

namespace HCore.Application.Modules.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = new Role(Guid.NewGuid(), dto.Name, "system");
            await _roleRepository.AddAsync(role);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<bool> UpdateRoleAsync(Guid id, CreateRoleDto dto)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return false;
            role.UpdateName(dto.Name);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return false;
            await _roleRepository.DeleteAsync(role);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
