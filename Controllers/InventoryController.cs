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
    public class InventoryController : ControllerBase
    {
        private readonly IIntentoryService _inventoryService;
        
        public InventoryController(IIntentoryService intentoryService)
        {
            _inventoryService = intentoryService;
        }
       
        [HttpPost("savepurchase")]
        public async Task<IActionResult> savePurchase(Purchase obj)
        {
            var data = await _inventoryService.savePurchase(obj);
            return Ok();
        }

        [HttpGet("getallpurchase")]
        public async Task<ActionResult<List<Purchase>>> getallpurchase()
        {
            var data = await _inventoryService.getallpurchase();
            return Ok(data);
        }

        [HttpGet("getallproducts")]
        public async Task<IActionResult> getallproducts(Purchase obj)
        {
            Console.WriteLine("Purchase Saved Successfullly");
            return Ok("Purchase Saved Successfullly");
        }

        [HttpGet("getallStock")]
        public async Task<IActionResult> getallStock(Purchase obj)
        {
            Console.WriteLine("Purchase Saved Successfullly");
            return Ok("Purchase Saved Successfullly");
        }

        [HttpGet("getallsell")]
        public async Task<IActionResult> getallsell(Purchase obj)
        {
            Console.WriteLine("Purchase Saved Successfullly");
            return Ok("Purchase Saved Successfullly");
        }
    }
}