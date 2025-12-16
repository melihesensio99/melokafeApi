using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Dtos.UserDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Validators.User;
using KafeApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly RegisterValidator _registerValidation;

        public UserService(IUserRepository userRepository, RegisterValidator registerValidation)
        {
            _userRepository = userRepository;
            _registerValidation = registerValidation;
        }

        public async Task<ResponseDto<object>> AddRole(string roleName)
        {
            try
            {
                var role = await _userRepository.AddRoleAsync(roleName);
                if (role)
                {
                    return new ResponseDto<object>
                    {
                        Message = "Yeni rol başarıyla oluşturuldu."
                    };
                }
                return new ResponseDto<object>
                {
                    Message = "Rol ekleme başarısız.",
                    ErrorCode = ErrorCodes.BADREQUEST,
                    Success = false
                };

            }
            catch (Exception)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem"
                };

            }

        }

        public async Task<ResponseDto<object>> AddRoleToUser(string email, string roleName)
        {
            try
            {
                var user = await _userRepository.AddRoleToUserAsync(email, roleName);
                if (user)
                {
                    return new ResponseDto<object>
                    {
                        Message = "Role kullaniciya başarıyla eklendi."
                    };
                }
                return new ResponseDto<object>
                {
                    Message = "Rol ekleme başarısız.",
                    ErrorCode = ErrorCodes.BADREQUEST,
                    Success = false
                };

            }
            catch (Exception)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem"
                };

            }
        }

        public async Task<ResponseDto<List<UserDto>>> GetUsersWithRole()
        {
            try
            {
                var result = await _userRepository.GetUsersWithRoleAsync();
                if (result != null)
                {
                    return new ResponseDto<List<UserDto>>
                    {
                        Data = result,
                        Message = "Kullanıcılar ve rolleri getirildi."
                    };
                }
                return new ResponseDto<List<UserDto>>
                {

                    Message = "Kullanıcılar bulunamadi",
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Success = false
                };
            }
            catch (Exception)
            {

                return new ResponseDto<List<UserDto>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem"
                };
            }
        }

        public async Task<ResponseDto<object>> Register(RegisterDto registerDto)
        {
            try
            {
                var checkValidation = await _registerValidation.ValidateAsync(registerDto);
                if (!checkValidation.IsValid)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Başarısız işlem!",
                        Data = null,
                        ErrorCode = ErrorCodes.VALIDATION_ERROR
                    };
                }
                var user = await _userRepository.RegisterAsync(registerDto);
                if (!user.Succeeded)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = user.Errors.FirstOrDefault().Description,
                        Data = null
                    };
                }
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Kayıt başarıyla tamamlandı."
                };

            }
            catch (Exception)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Başarısız işlem!!",
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Data = null
                };
            }
        }
    }
}

