using ClassWork_WEBAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshTokenEntity>
    {
        private AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<RefreshTokenEntity> GetTokenByNameAsync(string token)
        {
            return await GetAll().FirstOrDefaultAsync(t => t.Token == token);
        }
    }
}
