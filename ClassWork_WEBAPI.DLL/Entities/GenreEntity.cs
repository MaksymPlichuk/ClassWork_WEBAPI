using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DLL.Entities
{
    public class GenreEntity : BaseEntity
    {
        public required string Name { get; set; }
        public List<BookEntity> Books { get; set; } = [];
    }
}
