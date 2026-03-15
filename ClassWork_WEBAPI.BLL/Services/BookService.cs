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
        private readonly ImageService _imageService;
        public BookService(BookRepository repository, ImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
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
        public async Task<ServiceResponse> CreateBookAsync(CreateBookDto dto, string storageDir)
        {
            var e = new BookEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                Pages = dto.Pages,
                PublishYear = dto.PublishYear,
                Rating = dto.Rating
            };

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
            e.Title = dto.Title;
            e.Description = dto.Description;
            e.Pages = dto.Pages;
            e.PublishYear = dto.PublishYear;
            e.Rating = dto.Rating;

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
                Payload = new BookDto { Id = e.Id, Title = e.Title, Description = e.Description, Image = e.Image, Pages = e.Pages, PublishYear = e.PublishYear, Rating = e.Rating }
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
                Payload = new BookDto { Id = e.Id, Title = e.Title, Description = e.Description, Image = e.Image, Pages = e.Pages, PublishYear = e.PublishYear, Rating = e.Rating }
            };
        }
    }
}
