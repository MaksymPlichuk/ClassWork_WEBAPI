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
        public AuthorService(AuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse> CreateAuthorAsync(CreateAuthorDto dto)
        {
            var entity = new AuthorEntity
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Image = dto.Image,
                Country = dto.Country,
            };

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

        public async Task<ServiceResponse> UpdateAuthorAsync(UpdateAuthorDto dto)
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
            entity.Image = dto.Image;
            entity.BirthDate = dto.BirthDate;

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

        public async Task<ServiceResponse> DeleteAsync(int id)
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
