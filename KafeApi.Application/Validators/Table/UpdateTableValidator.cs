using FluentValidation;
using KafeApi.Application.Dtos.TableDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Validators.Table
{
    public class UpdateTableValidator : AbstractValidator<UpdateTableDto>
    {
        public UpdateTableValidator()
        {
            RuleFor(x => x.TableNumber).NotEmpty().WithMessage("Masa numarası boş bırakılamaz.").GreaterThan(0).WithMessage("Masa numarası 0dan büyük olmalıdır. ");

            RuleFor(x => x.TableCapacity).NotEmpty().WithMessage("Masa kapasitesi boş bırakılamaz.").GreaterThan(0).WithMessage("Masa kapasitesi 0dan büyük olmalıdır. ");
        }
    }
}
