using AutoMapper;
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
        private readonly IMapper _mapper;
        public AuthorService(AuthorRepository repository, ImageService imageService, IMapper mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAuthorAsync(CreateAuthorDto dto, string storageDir)
        {
            var entity = _mapper.Map<AuthorEntity>(dto);

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
                Payload = _mapper.Map<AuthorDto>(entity)
            };
        }
        public async Task<ServiceResponse> GetAllAsync()
        {
            var authors = _repository.GetAll();
            List<AuthorDto> dtos = _mapper.Map<List<AuthorDto>>(authors);

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
                    Payload = _mapper.Map<AuthorDto>(author)
                };
            }
            return new ServiceResponse
            {
                Message = $"Автор з Id {id} не знайдено",
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
            entity = _mapper.Map(dto, entity); //не створює новий об'єкт

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
                Payload = _mapper.Map<AuthorDto>(entity)
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
                Payload = _mapper.Map<AuthorDto>(entity)
            };
        }
    }
}
