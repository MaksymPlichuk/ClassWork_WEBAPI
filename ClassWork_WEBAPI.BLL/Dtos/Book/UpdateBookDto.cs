using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Dtos.Book
{
    public class UpdateBookDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public float Rating { get; set; } = 0f;
        [Required]
        public int Pages { get; set; } = 0;
        [Required]
        public int PublishYear { get; set; } = DateTime.UtcNow.Year;
    }
}
