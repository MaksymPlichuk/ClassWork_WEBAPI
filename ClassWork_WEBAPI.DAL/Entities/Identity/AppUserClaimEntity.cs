using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Entities.Identity
{
    public class AppUserClaimEntity : IdentityUserClaim<string>
    {
        public AppUserEntity? User { get; set; }
    }
}
