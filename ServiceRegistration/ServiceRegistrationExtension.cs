using inventoryApiDotnet.Context;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Repository;
using inventoryApiDotnet.Services;

namespace inventoryApiDotnet.Serviceregistration;

    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
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

            return services;
        }
    }