using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Dtos.Book
{
    public class CreateBookDto
    {
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Required]
        public float Rating { get; set; } = 0f;
        [Required]
        public int Pages { get; set; } = 0;
        [Required]
        public int PublishYear { get; set; } = DateTime.UtcNow.Year;
    }
}
