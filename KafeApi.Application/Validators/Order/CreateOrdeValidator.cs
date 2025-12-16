using FluentValidation;
using KafeApi.Application.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x=>x.OrderItems).NotEmpty().WithMessage("Lutfen itemOrder giriniz!");
        }
    }
}
