using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Dtos.Author
{
    public class UpdateAuthorDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public IFormFile? Image { get; set; }
        public string? Country { get; set; }
    }
}
