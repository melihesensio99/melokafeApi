using KafeApi.Application.Dtos.UserDtos;
using KafeApi.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace kafeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _userService.Register(registerDto);
            return CreateResponse(result);
        }
        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var result = await _userService.AddRole(roleName);
            return CreateResponse(result);
        }
        [HttpPost("addRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(string email ,string roleName)
        {
            var result = await _userService.AddRoleToUser(email ,roleName);
            return CreateResponse(result);
        }
        [HttpGet("getuserswithrole")]
        public async Task<IActionResult> GetUsersWithRole()
        {
            var result = await _userService.GetUsersWithRole();
            return CreateResponse(result);
        }
    }
}
