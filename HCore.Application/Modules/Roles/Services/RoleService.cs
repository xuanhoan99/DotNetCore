using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Roles.Interfaces;
using HCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCore.Application.Modules.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<RoleOuputDto> RoleById(string id)
        {
            RoleOuputDto roleOuputDto;
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                roleOuputDto = new RoleOuputDto();
            }
            else
            {
                roleOuputDto = new RoleOuputDto()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Desc = role.Desc
                };
            }
            return roleOuputDto;
        }

        public async Task<IdentityResult> RoleDel(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return await _roleManager.DeleteAsync(role);

        }

        public async Task<IdentityResult> RoleIns(RoleInsInputDto input)
        {
            var role = new Role()
            {
                Name = input.Name,
                NormalizedName = input.Name.ToUpper(),
                Desc = input.Desc
            };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<List<RoleOuputDto>> GetAllRole()
        {
            var roles = await _roleManager.Roles.Select(s => new RoleOuputDto()
            {
                Id = s.Id,
                Desc = s.Desc,
                Name = s.Name
            }).ToListAsync();
            return roles;
        }

        public async Task<IdentityResult> RoleUpd(RoleInputDto input)
        {
            var role = await _roleManager.FindByIdAsync(input.Id);
            role.Name = input.Name;
            role.NormalizedName = input.Name.ToUpper();
            role.Desc = input.Desc;
            return await _roleManager.UpdateAsync(role);
        }
    }
}
