using AutoMapper;
using ClassWork_WEBAPI.BLL.Dtos.Author;
using ClassWork_WEBAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.MapperProfiles
{
    public class AuthorMapperProfile : Profile
    {
        public AuthorMapperProfile()
        {
            //AuthorEntity --> AuthorDto
            CreateMap<AuthorEntity, AuthorDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country ?? "Невідома"));

            //CreateAuthorDto --> AuthorEntity
            CreateMap<CreateAuthorDto, AuthorEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            //UpdateAuthorDto --> AuthorEntity
            CreateMap<UpdateAuthorDto, AuthorEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
