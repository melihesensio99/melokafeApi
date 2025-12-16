using KafeApi.Application.Dtos.AuthDtos;
using KafeApi.Application.Dtos.UserDtos;
using KafeApi.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace kafeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("generateToken")]
        public async Task<IActionResult> GenerateToken(LoginDto loginDto)
        {
            var result = await _authService.GenerateToken(loginDto);
            return CreateResponse(result);
        }
    }
}
