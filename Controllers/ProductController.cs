using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Mvc;

namespace inventoryApiDotnet.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        ///  Gets all the existing Products from DB
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallProducts")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        } 

        /// <summary>
        /// Saves a new Product in the DB
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("saveProduct")]
        public async Task<ActionResult<Product>> SaveProduct(Product obj)
        {
            var result = await _productService.SaveProduct(obj);
            return Ok(result);
        }
    
    }
}