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
        private readonly IStockservice _stockservice;
        
        public InventoryController(IIntentoryService intentoryService,IStockservice stockservice)
        {
            _inventoryService = intentoryService;
            _stockservice = stockservice;
        }
       
        [HttpPost("savepurchase")]
        public async Task<IActionResult> savePurchase(Purchase obj)
        {
            await _inventoryService.savePurchase(obj);
            return Ok();
        }

        [HttpGet("getallpurchase")]
        public async Task<ActionResult<List<Purchase>>> getallpurchase()
        {
            var result = await _inventoryService.getallpurchase();
            return Ok(result);
        }

        [HttpGet("getallStock")]
        public async Task<ActionResult<Stock>> getallStock()
        {
            var result = await _stockservice.GetAllStock();
            return Ok(result);
        }

        [HttpGet("getallsell")]
        public async Task<IActionResult> getallsell(Purchase obj)
        {
            throw new NotFiniteNumberException();
        }
    }
}