using inventoryApiDotnet.Context;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Repository;
using inventoryApiDotnet.Services;

namespace inventoryApiDotnet.ServiceregistrationExtension;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIntentoryService, InventoryService>();
        services.AddScoped<IMongoContext, MongoContext>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<MongoDBService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IStockservice, Stockservice>();
        services.AddScoped<ISellRepository, SellRepository>();
        services.AddScoped<IInvoiceCounterService, InvoiceCounterService>();
        services.AddScoped<IInvoiceCounterRepository, InvoiceCounterRepository>();
        services.AddScoped<ISellItemRepository, SellItemRepository>();
        services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}