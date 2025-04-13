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

        public InventoryController(IIntentoryService intentoryService, IStockservice stockservice)
        {
            _inventoryService = intentoryService;
            _stockservice = stockservice;
        }

        /// <summary>
        /// Saves a new Purchase Record into Database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("savepurchase")]
        public async Task<IActionResult> SavePurchase(Purchase obj)
        {
            await _inventoryService.savePurchase(obj);
            return Ok();
        }

        [Obsolete]
        [HttpGet("getallpurchase_Obsolete")]
        public async Task<ActionResult<List<Purchase>>> GetAllPurchase()
        {
            var result = await _inventoryService.getallpurchase();
            return Ok(result);
        }

        /// <summary>
        /// Gets all the purchases according to pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getallpurchase")]
        public async Task<IActionResult> GetAllpurchase([FromQuery]int page = 1, [FromQuery]int pageSize = 100)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than 0." });
            }
            var result = await _inventoryService.getallpurchase(page, pageSize);
            return Ok(result);
        }

        [Obsolete]
        [HttpGet("getallStock_Obsolete")]
        public async Task<ActionResult<Stock>> GetAllStock()
        {
            var result = await _stockservice.GetAllStock();
            return Ok(result);
        }

        /// <summary>
        /// Gets all the Stock according to pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getallStock")]
        public async Task<ActionResult<Stock>> GetAllStock([FromQuery]int page = 1,[FromQuery]int pageSize = 100)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than 0." });
            }
            var result = await _stockservice.GetAllStock(page, pageSize);
            return Ok(result);
        }

        [Obsolete]
        [HttpGet("getallsell_Obsolete")]
        public async Task<IActionResult> GetAllSell()
        {
            var result = await _inventoryService.getallsell();
            return Ok(result);
        }

        /// <summary>
        /// Gets all the sell accoring to pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getallsell")]
        public async Task<IActionResult> GetAllSell([FromQuery]int page = 1,[FromQuery] int pageSize = 100)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than 0." });
            }
            var result = await _inventoryService.getallsell(page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Saves a new Sell record into Database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("savesell")]
        public async Task<IActionResult> SaveSell(Sell obj)
        {
            var response = await _inventoryService.saveNewSell(obj);
            if (response != "Success")
            {
                return BadRequest(response);
            }
            return Ok();
        }
    }
}