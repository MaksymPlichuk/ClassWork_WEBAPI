using ClassWork_WEBAPI.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Entities
{
    public class RefreshTokenEntity : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresDate { get; set; }
        public bool IsExpired { get; set; }
        public bool IsUsed { get; set; }
        public string UserId { get; set; }
        public AppUserEntity? User { get; set; }
    }
}
