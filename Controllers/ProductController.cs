using FluentValidation;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventoryApiDotnet.Controllers
{
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<Product> _validator;
        public ProductController(IProductService productService,
                                 IValidator<Product> validator)
        {
            _productService = productService;
            _validator = validator;
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
            var validationResult = await _validator.ValidateAsync(obj);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "New Product Details are not Valid.",
                    Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            var result = await _productService.SaveProduct(obj);
            return Ok(result);
        }   
    }
}