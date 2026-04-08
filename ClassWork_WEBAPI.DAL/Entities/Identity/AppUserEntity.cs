using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Entities.Identity
{
    public class AppUserEntity : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }

        public ICollection<AppUserClaimEntity> Claims { get; set; } = [];
        public ICollection<AppUserLoginEntity> Logins { get; set; } = [];
        public ICollection<AppUserTokenEntity> Tokens { get; set; } = [];
        public ICollection<AppUserRoleEntity> UserRoles { get; set; } = [];
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = [];
    }
}
