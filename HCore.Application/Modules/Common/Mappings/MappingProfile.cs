using AutoMapper;
using HCore.Application.Modules.Auth.Dtos;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.SysMenus.Dtos;
using HCore.Application.Modules.Users.Dtos;
using HCore.Domain.Entities;

namespace HCore.Application.Modules.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SysMenu, SysMenuDto>().ReverseMap();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<User, CurrentUserDto>().ReverseMap();

            CreateMap<Role, RoleInsInputDto>().ReverseMap();
            CreateMap<Role, RoleOuputDto>().ReverseMap();
        }
    }
}
