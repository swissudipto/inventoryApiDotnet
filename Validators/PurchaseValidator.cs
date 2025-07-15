
using FluentValidation;
using inventoryApiDotnet.Model;

internal sealed class PurchaseValidator : AbstractValidator<Purchase>
{
    public PurchaseValidator()
    {
        RuleFor(x => x.SupplierName)
        .NotEmpty()
        .WithMessage("Supplier Name Cannot be Empty")
        .NotNull()
        .WithMessage("Supplier Name Cannot be Null");

        RuleFor(x => x.PurchaseDate)
        .NotEmpty()
        .WithMessage("Purchase Date Cannot be Empty")
        .NotNull()
        .WithMessage("Purchase Date Cannot be Null");

        RuleFor(x => x.TotalAmount)
        .NotEmpty()
        .WithMessage("Total Amount Cannot be Empty")
        .NotNull()
        .WithMessage("Total Amount Cannot be Null")
        .GreaterThan(0)
        .WithMessage("Total Amount Should be greater than 0");

        RuleFor(x => x.purchaseItems)
        .NotEmpty()
        .WithMessage("The Purchase Items must Contain atleast 1 item");

        RuleForEach(x => x.purchaseItems).SetValidator(new PurchaseItemValidator());
    }
}