using ClassWork_WEBAPI.BLL.Dtos.Auth;
using ClassWork_WEBAPI.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class AuthService
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly JwtService _jwtService;
        public AuthService(UserManager<AppUserEntity> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }
        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        {
            if (await DoesEmailExist(dto.Email))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Email {dto.Email} вже зайнята!"
                };
            }
            if (await DoesUserNameExist(dto.UserName))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Username ${dto.UserName} вже зайнятий!"
                };
            }

            var user = new AppUserEntity
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var CreateRes = await _userManager.CreateAsync(user, dto.Password);

            if (!CreateRes.Succeeded)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = CreateRes.Errors.First().Description
                };
            }
            await _userManager.AddToRoleAsync(user, "user");

            return new ServiceResponse
            {
                Message = "Успішно зареєстровано",
            };

        }

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            if (!await DoesEmailExist(dto.Email))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Email {dto.Email} не зареєстрований!"
                };
            }
            var user = await _userManager.FindByEmailAsync(dto.Email);
            bool res = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Неправильний пароль"
                };
            }
            return new ServiceResponse
            {
                Message = "Успішний вхід",
                Payload = await _jwtService.GenerateTokenKey(user)
            };

        }

        private async Task<bool> DoesEmailExist(string email)
        {
            return (await _userManager.FindByEmailAsync(email) != null);
        }
        private async Task<bool> DoesUserNameExist(string userName)
        {
            return (await _userManager.FindByNameAsync(userName) != null);
        }
    }
}
