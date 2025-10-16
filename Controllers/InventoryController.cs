using FluentValidation;
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
        private readonly IValidator<Purchase> _purchaseValidator;
        private readonly IValidator<Sell> _sellValidator;

        public InventoryController(IIntentoryService intentoryService,
                                   IStockservice stockservice,
                                   IValidator<Purchase> purchaseValidator,
                                   IValidator<Sell> sellValidator)
        {
            _inventoryService = intentoryService;
            _stockservice = stockservice;
            _purchaseValidator = purchaseValidator;
            _sellValidator = sellValidator;
        }

        /// <summary>
        /// Saves a new Purchase Record into Database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("savepurchase")]
        public async Task<IActionResult> SavePurchase(Purchase obj)
        {
            var validationResult = await _purchaseValidator.ValidateAsync(obj);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Purchase Details are not Valid.",
                    Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            await _inventoryService.savePurchase(obj);
            return Ok();
        }

        /// <summary>
        /// Edits a new Purchase Record from Database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("editPurchase")]
        public async Task<IActionResult> editPurchase(Purchase obj)
        {
            var validationResult = await _purchaseValidator.ValidateAsync(obj);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Purchase Details are not Valid.",
                    Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            await _inventoryService.editPurchase(obj);
            return Ok();
        }

        /// <summary>
        /// Gets all the purchases according to pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getallpurchase")]
        public async Task<IActionResult> GetAllpurchase([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than 0." });
            }
            var result = await _inventoryService.getallpurchase(page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Gets all the Stock according to pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getallStock")]
        public async Task<ActionResult<Stock>> GetAllStock([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than 0." });
            }
            var result = await _stockservice.GetAllStock(page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Gets all the sell accoring to pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("getallsell")]
        public async Task<ActionResult<PagedResult<Sell>>> GetAllSell([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
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
            var validationResult = await _sellValidator.ValidateAsync(obj);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Sell Details are not Valid.",
                    Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            var response = await _inventoryService.saveNewSell(obj);
            if (response != "Success")
            {
                return BadRequest(response);
            }
            return Ok();
        }

        /// <summary>
        /// Edits a Sell record from Database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("editSell")]
        public async Task<IActionResult> editSell(Sell obj)
        {
            var validationResult = await _sellValidator.ValidateAsync(obj);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Sell Details are not Valid.",
                    Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            await _inventoryService.editSell(obj);
            return Ok();
        }
    }
}