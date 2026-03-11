using ClassWork_WEBAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Repositories
{
    public class BookRepository : GenericRepository<BookEntity>
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<BookEntity> GetByYear(int year)
        {
            return _context.Books.Where(b => b.PublishYear == year).ToList();
        }

        public List<BookEntity> GetByRating(float rating)
        {
            return _context.Books.Where(b => b.Rating == rating).ToList();
        }

        public List<BookEntity> GetByGenre(string genreName)
        {
            var genre = _context.Genres.AsNoTracking().FirstOrDefault(g => g.Name == genreName);
            if (genre != null)
            {
                return _context.Books.Where(b => b.Genres.Contains(genre)).ToList();
            }
            return null;
        }

        public List<BookEntity> GetByGenre(GenreEntity genre)
        {
            if (genre != null)
            {
                return _context.Books.Where(b => b.Genres.Contains(genre)).ToList();
            }
            return null;
        }

        public List<BookEntity> GetByAuthor(string authorName)
        {
            var author = _context.Authors.AsNoTracking().FirstOrDefault(a => a.Name == authorName);
            if (author != null)
            {
                return _context.Books.Where(b => b.Author == author).ToList();
            }
            return null;
        }
        public List<BookEntity> GetByAuthor(AuthorEntity author)
        {
            return _context.Books.Where(b => b.Author == author).ToList();
        }
    }
}
