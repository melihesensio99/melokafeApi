using FluentValidation;
using KafeApi.Application.Dtos.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Validators.OrderItem
{
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemDto>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Lütfen sipariş adeti giriniz!!").GreaterThan(0).WithMessage("Sipariş adeti 0dan büyük olmak zorundadir!!.");
        }
    }

}
