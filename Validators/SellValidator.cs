
using FluentValidation;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;

internal sealed class SellValidator : AbstractValidator<Sell>
{
    private readonly IStockservice _stockservice;
    public SellValidator(IStockservice stockservice)
    {
        _stockservice = stockservice;

        RuleFor(x => x.CustomerName)
        .NotEmpty()
        .WithMessage("Customer Name Cannot be Empty")
        .NotNull()
        .WithMessage("Customer Name Cannot be Null");

        RuleFor(x => x.SellDate)
        .NotEmpty()
        .WithMessage("Sell Date Cannot be Empty")
        .NotNull()
        .WithMessage("Sell Date Cannot be Null");

        RuleFor(x => x.TotalAmount)
        .NotEmpty()
        .WithMessage("Total Amount Cannot be Empty")
        .NotNull()
        .WithMessage("Total Amount Cannot be Null")
        .GreaterThan(0)
        .WithMessage("Total Amount Should be greater than 0");

        RuleFor(x => x.SellItems)
        .NotEmpty()
        .WithMessage("The Sell Items must Contain atleast 1 row")
        .MustAsync(BeinStockQuantities)
        .WithMessage("One or More item in list doesn't have enough stock. Please Check stock before selling!");

        RuleForEach(x => x.SellItems).SetValidator(new SellItemValidator());
    }

    public async Task<bool> BeinStockQuantities(List<SellItem>? SellItems,
                                                CancellationToken cancellationToken)
    {
        foreach (SellItem item in SellItems?? new List<SellItem>())
        {
            var stockResult = await _stockservice.GetStockByProductId(item.ProductId);
            if (stockResult.Quantity != null && item.Quantity > stockResult.Quantity)
            {
                return false;
            }
        }
        return true;
    }
}