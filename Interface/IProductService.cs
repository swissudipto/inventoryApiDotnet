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
        Task SaveProduct(Product obj);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}