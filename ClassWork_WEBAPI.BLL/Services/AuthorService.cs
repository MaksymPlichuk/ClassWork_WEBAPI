using ClassWork_WEBAPI.BLL.Dtos.Author;
using ClassWork_WEBAPI.DAL.Entities;
using ClassWork_WEBAPI.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _repository;
        private readonly ImageService _imageService;
        public AuthorService(AuthorRepository repository, ImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        public async Task<ServiceResponse> CreateAuthorAsync(CreateAuthorDto dto, string storageDir)
        {
            var entity = new AuthorEntity
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Country = dto.Country,
            };

            if (dto.Image != null)
            {
                var imageResponse = await _imageService.SaveImageAsync(dto.Image, storageDir);
                if (!imageResponse.Success) { return imageResponse; }

                entity.Image = imageResponse.Payload!.ToString();
            }

            var res = await _repository.CreateAsync(entity);
            if (!res)
            {
                return new ServiceResponse
                {
                    Message = "Невдалося додати автора",
                    Success = false
                };
            }

            return new ServiceResponse
            {
                Message = $"Автор {dto.Name} Успішно додано",
                Payload = new AuthorDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    BirthDate = entity.BirthDate,
                    Image = entity.Image,
                }
            };
        }
        public async Task<ServiceResponse> GetAllAsync()
        {
            var authors = _repository.GetAll();
            List<AuthorDto> dtos = new List<AuthorDto>();

            foreach (var a in authors)
            {
                var dto = new AuthorDto { Name = a.Name, Country = a.Country, BirthDate = a.BirthDate, Image = a.Image, Id = a.Id };
                dtos.Add(dto);
            }

            return new ServiceResponse
            {
                Message = $"{authors.Count()} Авторів знайдено",
                Payload = dtos
            };
        }
        public async Task<ServiceResponse> GetByIdAsync(int id)
        {

            var author = await _repository.GetByIdAsync(id);

            if (author != null)
            {
                return new ServiceResponse
                {
                    Message = $"Автор з Id {author.Id} знайдено",
                    Payload = new AuthorDto
                    {
                        Id = author.Id,
                        Name = author.Name,
                        BirthDate = author.BirthDate,
                        Image = author.Image,
                        Country = author.Country,
                    }
                };
            }
            return new ServiceResponse
            {
                Message = $"Автор з Id {author.Id} не знайдено",
                Success = false
            };
        }

        public async Task<ServiceResponse> UpdateAuthorAsync(UpdateAuthorDto dto, string storageDir)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Message = $"Автор з Id {dto.Id} не існує!",
                    Success = false
                };
            }

            string oldName = entity.Name;
            entity.Name = dto.Name;
            entity.Country = dto.Country;
            entity.BirthDate = dto.BirthDate;

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    var fullFilePath = Path.Combine(storageDir, entity.Image);
                    var deleteResp = _imageService.DeleteImage(fullFilePath);
                    if (!deleteResp.Success) { return deleteResp; }
                }

                var imageResponse = await _imageService.SaveImageAsync(dto.Image, storageDir);
                if (!imageResponse.Success) { return imageResponse; }

                entity.Image = imageResponse.Payload!.ToString();
            }

            bool res = await _repository.UpdateAsync(entity);
            if (!res)
            {
                return new ServiceResponse
                {
                    Message = "Невдалося оновити автора",
                    Success = false
                };
            }

            return new ServiceResponse
            {
                Message = $"Автор з {oldName} Успішно оновлений",
                Payload = new AuthorDto { Id = entity.Id, Name = entity.Name, BirthDate = entity.BirthDate, Country = entity.Country, Image = entity.Image }
            };
        }

        public async Task<ServiceResponse> DeleteAsync(int id, string storageDir)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return new ServiceResponse
                {
                    Message = $"Автора з Id {id} не існує",
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(entity.Image))
            {
                var fullFilePath = Path.Combine(storageDir, entity.Image);
                var imageResponse = _imageService.DeleteImage(fullFilePath);

                if (!imageResponse.Success) { return imageResponse; }
            }

            bool res = await _repository.DeleteAsync(id);

            if (!res)
            {
                return new ServiceResponse
                {
                    Message = $"Невдалося видалити автора",
                    Success = false
                };
            }
            return new ServiceResponse
            {
                Message = $"Автор з Id {id} успішно видалено",
                Payload = new AuthorDto { Id = entity.Id, Name = entity.Name, BirthDate = entity.BirthDate, Country = entity.Country, Image = entity.Image }
            };
        }
    }
}
