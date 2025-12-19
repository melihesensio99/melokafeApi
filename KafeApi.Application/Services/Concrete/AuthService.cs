using KafeApi.Application.Dtos.AuthDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Dtos.UserDtos;
using KafeApi.Application.Helpers;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly TokenHelpers _tokenHelpers;
        private readonly IUserRepository _userRepository;


        public AuthService(TokenHelpers tokenHelpers, IUserRepository userRepository)
        {
            _tokenHelpers = tokenHelpers;
            _userRepository = userRepository;


        }

        public async Task<ResponseDto<object>> GenerateToken(LoginDto loginDto)
        {
            var userCheck = await _userRepository.CheckUserAsync(loginDto.Email);
            if (userCheck.Id != null)
            {
                var user = await _userRepository.CheckUserWithPasswordAsync(loginDto);
                if (user)
                {
                    var tokenDto = new TokenDto
                    {
                        Email = loginDto.Email,
                        Id = userCheck.Id,
                        Role = userCheck.Role
                    };
                    var token = _tokenHelpers.GenerateToken(tokenDto);
                    return new ResponseDto<object>
                    {
                        Success = true,
                        Data = token
                    };
                }

            }
            return new ResponseDto<object> { Success = false , ErrorCode = ErrorCodes.VALIDATION_ERROR ,  Message = "Hatalı Giriş!"};
            

        }
    }
}
