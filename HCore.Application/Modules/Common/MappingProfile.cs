using AutoMapper;
using HCore.Application.Modules.Users.Dtos;
using HCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCore.Application.Modules.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Định nghĩa ánh xạ giữa các DTO và Entity tại đây
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
