using FluentValidation;
using inventoryApiDotnet.Model;

internal sealed class SellItemValidator : AbstractValidator<SellItem>
{
    public SellItemValidator()
    {
        RuleFor(x => x.ProductName)
        .NotEmpty()
        .WithMessage("Product Name cannot be Empty")
        .NotNull()
        .WithMessage("Product Name connect be null");

        RuleFor(x => x.ProductId)
        .NotEmpty()
        .WithMessage("Product Id cannot be Empty")
        .NotNull()
        .WithMessage("Product Id connect be null");

        RuleFor(x => x.Quantity)
        .NotEmpty()
        .WithMessage("Item Quantity cannot be Empty")
        .NotNull()
        .WithMessage("Item Quantity connect be null")
        .GreaterThan(0)
        .WithMessage("Item Quantity Should be Greater than 0");

        RuleFor(x => x.Amount)
        .NotEmpty()
        .WithMessage("Item Amount cannot be Empty")
        .NotNull()
        .WithMessage("Item Amount connect be null")
        .GreaterThan(0)
        .WithMessage("Item Amount Should be Greater than 0");
    }
}