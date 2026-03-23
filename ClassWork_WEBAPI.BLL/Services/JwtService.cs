using ClassWork_WEBAPI.BLL.Dtos.Auth;
using ClassWork_WEBAPI.BLL.Settings;
using ClassWork_WEBAPI.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<AppRoleEntity> _roleManager;
        public JwtService(IOptions<JwtSettings> jwtOptions, RoleManager<AppRoleEntity> roleManager)
        {
            _jwtSettings = jwtOptions.Value;
            _roleManager = roleManager;
        }

        public async Task<string> GenerateTokenKey(AppUserEntity user)
        {
            if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
            {
                throw new Exception("Немає SecretKey");
            }

            string roleName;
            //if (user.UserRoles.Any())
            //{
            //    roleName = "No roles";
            //}
            //else
            //{
            //    roleName = await _roleManager.GetRoleNameAsync(user.UserRoles.FirstOrDefault().Role);
            //}

            var claims = new List<Claim> {
                new Claim("userName", user.UserName ?? string.Empty),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
                new Claim("image", user.Image ?? string.Empty),
                new Claim("role", roleName ?? string.Empty)
            };


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
