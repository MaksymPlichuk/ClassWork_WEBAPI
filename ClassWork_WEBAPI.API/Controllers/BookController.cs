using ClassWork_WEBAPI.API.Extensions;
using ClassWork_WEBAPI.API.Settings;
using ClassWork_WEBAPI.BLL.Dtos.Book;
using ClassWork_WEBAPI.BLL.Dtos.Pagination;
using ClassWork_WEBAPI.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClassWork_WEBAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly string _storagePath;
        public BookController(BookService bookService, IWebHostEnvironment environment)
        {
            _bookService = bookService;
            string rootPath = environment.ContentRootPath;
            _storagePath = Path.Combine(rootPath, StaticFilesSettings.StorageDir, StaticFilesSettings.BooksDir);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]PaginationDto pagination)
        {
            var resp = await _bookService.GetAllAsync(pagination);
            return this.GetAction(resp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var resp = await _bookService.GetByIdAsync(id);
            return this.GetAction(resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBookDto dto)
        {
            var resp = await _bookService.CreateBookAsync(dto, _storagePath);
            return this.GetAction(resp);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateBookDto dto)
        {
            var resp = await _bookService.UpdateBookAsync(dto, _storagePath);
            return this.GetAction(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var resp = await _bookService.DeleteAsync(id, _storagePath);
            return this.GetAction(resp);
        }
    }
}
