using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class ImageService
    {
        public async Task<ServiceResponse> SaveImageAsync(IFormFile file, string storagePath)
        {
            try
            {
                var type = file.ContentType.Split("/");
                if (type.Length != 2 || type[0] != "image")
                {
                    return new ServiceResponse
                    {
                        Message = "Файл не є картинкою",
                        Success = false
                    };
                }
                string imageName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string fullSavePath = Path.Combine(storagePath, imageName);

                using var fileStream = File.OpenWrite(fullSavePath);
                await file.CopyToAsync(fileStream);

                return new ServiceResponse
                {
                    Message = "Зображення успішно додано",
                    Payload = imageName
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }
            
        }

        public ServiceResponse DeleteImage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new ServiceResponse
                {
                    Message = $"Файла за адресою '{filePath}' не існує!",
                    Success = false
                };
            }
            File.Delete(filePath);
            return new ServiceResponse { Message = $"Файл за адресою '{filePath}' успішно видалено!" };
        }
    }
}
