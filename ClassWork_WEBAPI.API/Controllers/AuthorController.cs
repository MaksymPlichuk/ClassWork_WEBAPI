using ClassWork_WEBAPI.API.Extensions;
using ClassWork_WEBAPI.API.Settings;
using ClassWork_WEBAPI.BLL.Dtos.Author;
using ClassWork_WEBAPI.BLL.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClassWork_WEBAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;
        private readonly string _storagePath;

        public AuthorController(AuthorService authorService, IWebHostEnvironment environment)
        {
            _authorService = authorService;

            string rootPath = environment.ContentRootPath;
            _storagePath = Path.Combine(rootPath, StaticFilesSettings.StorageDir, StaticFilesSettings.AuthorsDir);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var resp = await _authorService.GetAllAsync();
            return this.GetAction(resp);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var resp = await _authorService.GetByIdAsync(id);
            return this.GetAction(resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateAuthorDto dto)
        {
            var res = await _authorService.CreateAuthorAsync(dto, _storagePath);
            return this.GetAction(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateAuthorDto dto)
        {
            var resp = await _authorService.UpdateAuthorAsync(dto, _storagePath);
            return this.GetAction(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var resp = await _authorService.DeleteAsync(id, _storagePath);
            return this.GetAction(resp);
        }
    }
}
