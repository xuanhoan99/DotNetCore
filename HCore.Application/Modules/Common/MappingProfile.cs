using AutoMapper;
using HCore.Application.Modules.SysMenus.Dtos;
using HCore.Domain.Entities;

namespace HCore.Application.Modules.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SysMenu, SysMenuDto>().ReverseMap();
        }
    }
}
