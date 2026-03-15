using AutoMapper;
using ClassWork_WEBAPI.BLL.Dtos.Book;
using ClassWork_WEBAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.MapperProfiles
{
    public class BookMapperProfile : Profile
    {
        public BookMapperProfile()
        {
            CreateMap<BookEntity, BookDto>();

            CreateMap<CreateBookDto, BookEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<UpdateBookDto, BookEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
