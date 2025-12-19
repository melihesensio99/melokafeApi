using KafeApi.Application.Dtos.UserDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Persistence.Context.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;

        public UserRepository(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, RoleManager<AppIdentityRole> appIdentityRole)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = appIdentityRole;
        }

        public async Task<bool> AddRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return false;
            var roleCheck = await _roleManager.RoleExistsAsync(roleName);
            if (roleCheck)
                return false;
            var result = await _roleManager.CreateAsync(new AppIdentityRole { Name = roleName });
            if (result.Succeeded)
                return true;
            return false;

        }

        public async Task<bool> AddRoleToUserAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist)
            {
                var addRole = await _userManager.AddToRoleAsync(user, roleName);
                if (addRole.Succeeded)
                    return true;
            }
            return false;
        }

        public async Task<UserDto> CheckUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                return new UserDto { Id = user.Id, Email = user.Email , Role = role.FirstOrDefault() };
            }
            return null;

        }

        public async Task<bool> CheckUserWithPasswordAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return false;
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            return result;


        }

        public async Task<List<UserDto>> GetUsersWithRoleAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var list = new List<UserDto>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u); 
                list.Add(new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = string.Join(", ", roles)
                });
            }
            return list;
        }

        public async Task<SignInResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, true, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppIdentityUser
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                PhoneNumber = registerDto.Phone,
                UserName = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }


    }
}
