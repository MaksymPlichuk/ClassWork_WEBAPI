using ClassWork_WEBAPI.BLL.Dtos.Auth;
using ClassWork_WEBAPI.BLL.Settings;
using ClassWork_WEBAPI.DAL.Entities;
using ClassWork_WEBAPI.DAL.Entities.Identity;
using ClassWork_WEBAPI.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly RefreshTokenRepository _refreshTokenRepository;
        public JwtService(IOptions<JwtSettings> jwtOptions, UserManager<AppUserEntity> userManager, RefreshTokenRepository refreshTokenRepository)
        {
            _jwtSettings = jwtOptions.Value;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }

        private async Task<RefreshTokenEntity> GenerateRefreshTokenAsync()
        {
            byte[] bytes = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            string token = Convert.ToBase64String(bytes);
            return new RefreshTokenEntity
            {
                Token = token,
                ExpiresDate = DateTime.UtcNow.AddDays(7)
            };

        }

        public async Task<JwtDto> GenerateTokensAsync(AppUserEntity user)
        {
            string accessToken = await GenerateTokenKey(user);
            var refreshToken = await GenerateRefreshTokenAsync();
            refreshToken.UserId = user.Id;
            await _refreshTokenRepository.CreateAsync(refreshToken);

            return new JwtDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
        public async Task<ServiceResponse> RefreshAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetTokenByNameAsync(refreshToken);
            if (token == null || token.IsUsed || token.IsExpired)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Неправильний JWT token"
                };
            }
            var user = await _userManager.FindByIdAsync(token.UserId);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Неправильний JWT token"
                };
            }

            token.IsExpired = true;
            await _refreshTokenRepository.UpdateAsync(token);

            var newTokens = await GenerateTokensAsync(user);
            return new ServiceResponse
            {
                Message = "Успішно оновлено токени",
                Payload = newTokens
            };

        }

        private async Task<string> GenerateTokenKey(AppUserEntity user)
        {
            if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
            {
                throw new Exception("Немає SecretKey");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                new Claim("userName", user.UserName ?? string.Empty),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
                new Claim("image", user.Image ?? string.Empty),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var bytesSecretKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var signinKey = new SymmetricSecurityKey(bytesSecretKey);

            var credentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddHours(_jwtSettings.ExpHours)
                );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
