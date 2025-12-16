using KafeApi.Application.Dtos.AuthDtos;
using KafeApi.Application.Dtos.UserDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<SignInResult> LoginAsync(LoginDto loginDto);
        Task LogoutAsync();
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> CheckUserAsync(string email );
        Task<bool> CheckUserWithPasswordAsync(LoginDto loginDto);
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> AddRoleToUserAsync(string email , string roleName);
        Task<List<UserDto>> GetUsersWithRoleAsync();
    }
}
