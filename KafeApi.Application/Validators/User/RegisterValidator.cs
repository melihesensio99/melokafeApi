using FluentValidation;
using KafeApi.Application.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Validators.User
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ad alanı boş olamaz.")
            .MinimumLength(2)
            .WithMessage("Ad alanı en az 2 karakter olmalıdır.");


            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Soyad alanı boş olamaz.")
                .MinimumLength(2)
                .WithMessage("Soyad alanı en az 2 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email alanı boş olamaz.")
                .EmailAddress()
                .WithMessage("Geçersiz email adresi.");

        //    RuleFor(x => x.Password)
        //        .NotEmpty()
        //    .WithMessage("Şifre alanı boş olamaz.")
        //.MinimumLength(6)
        // .WithMessage("Şifre en az 6 karakter olmalıdır.")
        //   .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$")
        //       .WithMessage("Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.");
        }
    }
}
