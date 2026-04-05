using AutoMapper;
using ClassWork_WEBAPI.BLL.Dtos.Book;
using ClassWork_WEBAPI.BLL.Dtos.Pagination;
using ClassWork_WEBAPI.DAL.Entities;
using ClassWork_WEBAPI.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class BookService : PaginationService
    {
        private readonly BookRepository _repository;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;
        public BookService(BookRepository repository, ImageService imageService, IMapper mapper)
            : base(mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync(PaginationDto pagination)
        {
            var entities = _repository.GetAll().Include(b => b.Author);
            var resp = await GetPaginationAsync<BookEntity, BookDto>(entities, pagination);

            return new ServiceResponse
            {
                Message = $"{resp.Data.Count()} Книг Знайдено",
                Payload = resp
            };
        }
        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var e = await _repository.GetByIdAsync(id);
            if (e == null)
            {
                return new ServiceResponse
                {
                    Message = $"Книги з Id {id} не існує",
                    Success = false
                };
            }
            return new ServiceResponse
            {
                Message = $"Книга з Id {id} знайдена!",
                Payload = _mapper.Map<BookDto>(e)
            };
        }
        public async Task<ServiceResponse> CreateBookAsync(CreateBookDto dto, string storageDir)
        {
            var e = _mapper.Map<BookEntity>(dto);

            if (dto.Image != null)
            {
                var imgResponse = await _imageService.SaveImageAsync(dto.Image, storageDir);
                if (!imgResponse.Success) { return imgResponse; }
                e.Image = imgResponse.Payload!.ToString();
            }

            var res = await _repository.CreateAsync(e);
            if (!res)
            {
                return new ServiceResponse
                {
                    Message = "Невдалося додати книгу",
                    Success = false
                };
            }

            return new ServiceResponse
            {
                Message = "Книга успішно додана",
                Payload = _mapper.Map<BookDto>(e)
            };
        }
        public async Task<ServiceResponse> UpdateBookAsync(UpdateBookDto dto, string storageDir)
        {
            var e = await _repository.GetByIdAsync(dto.Id);
            if (e == null)
            {
                return new ServiceResponse
                {
                    Message = $"Книга з Id {dto.Id} не існує",
                    Success = false
                };
            }

            string oldTitle = e.Title;
            e = _mapper.Map(dto, e);

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(e.Image))
                {
                    var fullFilePath = Path.Combine(storageDir, e.Image);
                    var deleteResp = _imageService.DeleteImage(fullFilePath);
                    if (!deleteResp.Success) { return deleteResp; }
                }

                var imgResponse = await _imageService.SaveImageAsync(dto.Image, storageDir);
                if (!imgResponse.Success) { return imgResponse; }
                e.Image = imgResponse.Payload!.ToString();
            }



            bool res = await _repository.UpdateAsync(e);
            if (!res)
            {
                return new ServiceResponse
                {
                    Message = $"Невдалося оновити книгу",
                    Success = false
                };
            }

            return new ServiceResponse
            {
                Message = $"Книга '{oldTitle}' успішно змінена!",
                Payload = _mapper.Map<BookDto>(e)
            };
        }
        public async Task<ServiceResponse> DeleteAsync(int id, string storageDir)
        {
            var e = await _repository.GetByIdAsync(id);
            if (e == null)
            {
                return new ServiceResponse
                {
                    Message = $"Книга з Id {id} не існує",
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(e.Image))
            {
                var fullFilePath = Path.Combine(storageDir, e.Image);
                var imgResponse = _imageService.DeleteImage(fullFilePath);
                if (!imgResponse.Success) { return imgResponse; }
            }

            bool res = await _repository.DeleteAsync(e);
            if (!res)
            {
                return new ServiceResponse
                {
                    Message = $"Невдалося видалити книгу",
                    Success = false
                };
            }

            return new ServiceResponse
            {
                Message = $"Книга '{e.Title}' успішно видалена!",
                Payload = _mapper.Map<BookDto>(e)
            };
        }
    }
}
