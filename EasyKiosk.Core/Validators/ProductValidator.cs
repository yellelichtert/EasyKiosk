using EasyKiosk.Core.Entities;
using FluentValidation;

namespace EasyKiosk.Core.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .NotNull();

        RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .NotNull();
    }
}