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
        public async Task<List<BookEntity>> GetBooksAsync(AuthorEntity entity)
        {
            return await _context.Books.AsNoTracking().Where(b => b.AuthorId == entity.Id).ToListAsync();
        }
        public async Task<bool> AddBookAsync(AuthorEntity author, BookEntity book)
        {
            var authorBooks = await GetBooksAsync(author);

            if (!authorBooks.Contains(book))
            {
                author.Books.Add(book);
                return (await _context.SaveChangesAsync()) != 0;
            }
            return false;
        }

    }
}
