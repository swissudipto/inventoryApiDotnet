
using FluentValidation;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;

internal sealed class SellValidator : AbstractValidator<Sell>
{
    private readonly IStockservice _stockservice;
    private readonly IIntentoryService _inventoryService;
    public SellValidator(IStockservice stockservice,
                         IIntentoryService inventoryService)
    {
        _stockservice = stockservice;
        _inventoryService = inventoryService;

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

        RuleFor(x => x)
        .MustAsync(async (sell, cancellation) => await BeinStockQuantities(sell))
        .WithMessage("One or More item in list doesn't have enough stock. Please Check stock before selling!");

        RuleForEach(x => x.SellItems).SetValidator(new SellItemValidator());
    }

    private async Task<bool> BeinStockQuantities(Sell sell)
    {
        var response = await _inventoryService.GetSellDetailsfromDB(sell.Id ?? "");
        var isEditMode = response.Count == 0 ? false : true;
        if (isEditMode)
        {
            var newSellItems = sell.SellItems;
            var oldSellItems = response?.FirstOrDefault()?.SellItems;


            foreach (var item in newSellItems ?? new List<SellItem>())
            {
                var matchedOldItem = oldSellItems?.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                if (matchedOldItem != null)
                {
                    if (item.Quantity > matchedOldItem.Quantity)
                    {
                        var stockResult = await _stockservice.GetStockByProductId(item.ProductId);

                        if (stockResult.Quantity != null && (item.Quantity - matchedOldItem.Quantity) > stockResult.Quantity)
                        {
                            return false;
                        }
                    }
                    oldSellItems?.Remove(matchedOldItem);
                }
                else
                {
                    string message;
                    var isProductInStock =  _stockservice.checkIfProductInStock(item, out message);
                    if (!isProductInStock)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        else
        {
            foreach (SellItem item in sell.SellItems ?? new List<SellItem>())
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
}