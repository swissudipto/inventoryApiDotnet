using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;

namespace inventoryApiDotnet.Interface
{
    public interface IProductService
    {
        Task<Product> SaveProduct(Product obj);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(long productID);
    }
}