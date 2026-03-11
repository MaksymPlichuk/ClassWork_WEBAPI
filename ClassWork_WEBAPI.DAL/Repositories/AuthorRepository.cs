using ClassWork_WEBAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Repositories
{
    public class AuthorRepository : GenericRepository<AuthorEntity>
    {
        private readonly AppDbContext _context;
        public AuthorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<AuthorEntity?> GetByNameAsync(string name)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());
        }
        //todo
        public async Task<List<BookEntity>> GetBooksAsync() { return null; }
        public async Task<bool> AddBookAsync() { return false; }

    }
}
