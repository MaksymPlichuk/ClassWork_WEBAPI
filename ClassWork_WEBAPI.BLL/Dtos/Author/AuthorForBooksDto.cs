using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Dtos.Author
{
    public class AuthorForBooksDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int BooksCount { get; set; } = 0;
    }
}
