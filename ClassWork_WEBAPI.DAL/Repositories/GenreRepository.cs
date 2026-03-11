using ClassWork_WEBAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Repositories
{
    public class GenreRepository : GenericRepository<GenreEntity>
    {
        private readonly AppDbContext _context;
        public GenreRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public GenreEntity GetByName(string name)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.Name == name);

            if (genre == null) { return null; }

            return genre;
        }
    }
}
