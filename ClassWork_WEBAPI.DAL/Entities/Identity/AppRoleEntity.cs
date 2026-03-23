using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Entities.Identity
{
    public class AppRoleEntity : IdentityRole
    {
        public ICollection<AppUserRoleEntity> UserRoles { get; set; } = [];
        public ICollection<AppRoleClaimEntity> RoleClaims { get; set; } = [];
    }
}
