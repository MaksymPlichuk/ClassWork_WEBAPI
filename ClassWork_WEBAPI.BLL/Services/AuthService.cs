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
        private readonly EmailService _emailService;
        public AuthService(UserManager<AppUserEntity> userManager, JwtService jwtService, EmailService emailService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<ServiceResponse> EmailConfirmationAsync(string uid, string base64Token)
        {
            var user = await _userManager.FindByIdAsync(uid);
            if (user == null) {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Користувача з uid '{uid}' не знайдено"
                };
            }

            byte[] bytes = Convert.FromBase64String(base64Token);
            string token = Encoding.UTF8.GetString(bytes);

            var resp = await _userManager.ConfirmEmailAsync(user, token);

            if (!resp.Succeeded)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = resp.Errors.First().Description
                };
            }
            return new ServiceResponse { Message = "Пошта успішно підтверджена!" };
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

            //Confirmation Email
            
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] bytes = Encoding.UTF8.GetBytes(token);
            string base64Token = Convert.ToBase64String(bytes);

            string root = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(root, "Storage", "Templates", "ConfirmationEmail.html");
            if (File.Exists(filePath))
            {
                string html = await File.ReadAllTextAsync(filePath);
                html = html.Replace("{id}", user.Id);
                html = html.Replace("{token}", base64Token);
                await _emailService.SendEmailAsync(user.Email, "Підтвердження Пошти", html, true);
            }

            return new ServiceResponse
            {
                Message = "Успішно зареєстровано",
                Payload = $"https://localhost:7211/confirmEmail?uid={user.Id}&t={base64Token}"
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
