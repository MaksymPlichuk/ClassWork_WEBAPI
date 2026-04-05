using ClassWork_WEBAPI.BLL.Dtos.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Dtos.Author
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public string? Image { get; set; }
        public string? Country { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
