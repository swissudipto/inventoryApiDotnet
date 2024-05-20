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
   
        [HttpGet("getallProducts")]
        public async Task<ActionResult<List<Product>>> getallProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        } 

        [HttpPost("saveProduct")]
        public async Task<IActionResult> saveProduct(Product obj)
        {
            await _productService.SaveProduct(obj);
            return Ok();
        }
    
    }
}