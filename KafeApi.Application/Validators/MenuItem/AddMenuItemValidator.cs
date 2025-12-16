using FluentValidation;
using KafeApi.Application.Dtos.MenuItemsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Validators.MenuItem
{
    public class AddMenuItemValidator : AbstractValidator<CreateMenuItemDto>
    {
        public AddMenuItemValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen isim giriniz.")
                .Length(3, 30).WithMessage("Lutfen 3-30 karakter arasi bir deger giriniz.");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Lutfen alani doldurunuz.")
                .GreaterThan(0).WithMessage("Lutfen 0dan buyuk bir deger giriniz.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Lütfen aciklama giriniz.")
                .Length(1, 100).WithMessage("Lutfen 1-100 karakter arasi bir deger giriniz.");

            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Lütfen imageURL giriniz.");





        }
    }
}
