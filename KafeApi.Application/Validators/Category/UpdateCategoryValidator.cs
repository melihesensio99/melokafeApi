using FluentValidation;
using KafeApi.Application.Dtos.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Validators.Category
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x=>x.CategoryName).Length(3,30).WithMessage("Kategori adi 3-30 Karakter arasi olmalidir.").NotEmpty().WithMessage("Kategori adi boş birakilamaz.");   

        }
    }
}
