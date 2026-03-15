using ClassWork_WEBAPI.BLL.Dtos.Book;
using ClassWork_WEBAPI.DAL.Entities;
using ClassWork_WEBAPI.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class BookService
    {
        private readonly BookRepository _repository;
        public BookService(BookRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = _repository.GetAll();
            List<BookDto> books = new List<BookDto>();

            foreach (var e in entities)
            {
                var dto = new BookDto { Id = e.Id, Title = e.Title, Description = e.Description, Image = e.Image, Pages = e.Pages, PublishYear = e.PublishYear, Rating = e.Rating };
                books.Add(dto);
            }
            return new ServiceResponse
            {
                Message = $"{books.Count()} Книг Знайдено",
                Payload = books
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
                Payload = new BookDto { Id = e.Id, Title = e.Title, Description = e.Description, Image = e.Image, Pages = e.Pages, PublishYear = e.PublishYear, Rating = e.Rating }
            };
        }
        public async Task<ServiceResponse> CreateBookAsync(CreateBookDto dto)
        {
            var e = new BookEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                Image = dto.Image,
                Pages = dto.Pages,
                PublishYear = dto.PublishYear,
                Rating = dto.Rating
            };
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
                Payload = new BookDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Image = e.Image,
                    Pages = e.Pages,
                    PublishYear = e.PublishYear,
                    Rating = e.Rating
                }
            };
        }
        public async Task<ServiceResponse> UpdateBookAsync(UpdateBookDto dto)
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
            e.Title = dto.Title;
            e.Description = dto.Description;
            e.Image = dto.Image;
            e.Pages = dto.Pages;
            e.PublishYear = dto.PublishYear;
            e.Rating = dto.Rating;

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
                Payload = new BookDto { Id = e.Id, Title = e.Title, Description = e.Description, Image = e.Image, Pages = e.Pages, PublishYear = e.PublishYear, Rating = e.Rating }
            };
        }
        public async Task<ServiceResponse> DeleteAsync(int id)
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
                Payload = new BookDto { Id = e.Id, Title = e.Title, Description = e.Description, Image = e.Image, Pages = e.Pages, PublishYear = e.PublishYear, Rating = e.Rating }
            };
        }
    }
}
