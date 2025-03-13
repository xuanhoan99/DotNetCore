using AutoMapper;
using HCore.Application.Modules.Roles.Dtos;
using HCore.Application.Modules.Users.Dtos;
using HCore.Domain.Entities;

namespace HCore.Application.Modules.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Định nghĩa ánh xạ giữa các DTO và Entity tại đây
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }
    }
}
