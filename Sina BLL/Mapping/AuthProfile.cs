using AutoMapper;
using Sina_BLL.DTO.AuthintcationDto;
using Sina_DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_BLL.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<UserRegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                           opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }

}
