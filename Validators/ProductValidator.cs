using FluentValidation;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;

internal sealed class ProductValidator : AbstractValidator<Product>
{
    private readonly IProductService _productService;
    public ProductValidator(IProductService productService)
    {
        _productService = productService;

        RuleFor(X => X.ProductName)
        .NotEmpty()
        .WithMessage("Product Name Cannot be empty on New Product save!")
        .NotNull()
        .WithMessage("Product Name Cannot be null on New Product save!")
        .MustAsync(BeAUniqueProductName)
        .WithMessage("The Product is already exists in our System.Enter a New Product Name!");
    }

    public async Task<bool> BeAUniqueProductName(string? name, CancellationToken cancellationToken)
    {
        var result = await _productService.IsProductNameExists(name);
        return !result;
    }
}