using ClassWork_WEBAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Dtos.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float Rating { get; set; } = 0f;
        public int Pages { get; set; } = 0;
        public int PublishYear { get; set; } = DateTime.UtcNow.Year;

    }
}
