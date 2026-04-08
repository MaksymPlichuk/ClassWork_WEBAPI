using ClassWork_WEBAPI.API.Extensions;
using ClassWork_WEBAPI.BLL.Dtos.Auth;
using ClassWork_WEBAPI.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClassWork_WEBAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;
        public AuthController(AuthService authService, JwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var resp = await _authService.RegisterAsync(dto);
            return this.GetAction(resp);
        }
        [HttpPost("/login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var resp = await _authService.LoginAsync(dto);
            return this.GetAction(resp);
        }
        [HttpPost("/refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] string refreshToken)
        {
            var resp = await _jwtService.RefreshAsync(refreshToken);
            return this.GetAction(resp);
        }

        [HttpGet("/confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string uid, [FromQuery] string t)
        {
            var resp = await _authService.EmailConfirmationAsync(uid, t);
            return this.GetAction(resp);
        }
    }
}
