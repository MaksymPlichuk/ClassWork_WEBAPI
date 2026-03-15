using ClassWork_WEBAPI.API.Extensions;
using ClassWork_WEBAPI.BLL.Dtos.Book;
using ClassWork_WEBAPI.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClassWork_WEBAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var resp = await _bookService.GetAllAsync();
            return this.GetAction(resp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var resp = await _bookService.GetByIdAsync(id);
            return this.GetAction(resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBookDto dto)
        {
            var resp = await _bookService.CreateBookAsync(dto);
            return this.GetAction(resp);
        }

        [HttpPut]
        public async Task<IActionResult> CreateAsync([FromBody] UpdateBookDto dto)
        {
            var resp = await _bookService.UpdateBookAsync(dto);
            return this.GetAction(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var resp = await _bookService.DeleteAsync(id);
            return this.GetAction(resp);
        }
    }
}
