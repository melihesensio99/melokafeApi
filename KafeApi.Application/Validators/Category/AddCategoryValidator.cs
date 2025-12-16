using FluentValidation;
using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Validators.Category
{
    public class AddCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public AddCategoryValidator() 
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adi boş birakilamaz.")
                .Length(3, 30).WithMessage("Kategori adi 3-30 karakter arasi olmalidir.");


        }
    }
}
