using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Dtos.UserDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Abstract
{
    public interface IUserService
    {
        Task<ResponseDto<object>> Register(RegisterDto registerDto);
        Task<ResponseDto<object>> AddRole(string roleName);
        Task<ResponseDto<object>> AddRoleToUser(string email, string roleName);
        Task<ResponseDto<List<UserDto>>> GetUsersWithRole();

    }
}
