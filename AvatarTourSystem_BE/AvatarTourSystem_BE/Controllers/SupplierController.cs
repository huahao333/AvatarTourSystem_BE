using BusinessObjects.Models;
using BusinessObjects.ViewModels.Supplier;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierlService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierlService = supplierService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetListActiveSuppliersAsync()
        {
            var result = await _supplierlService.GetActiveSuppliersAsync();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetListSuppliersAsync()
        {
            var result = await _supplierlService.GetSuppliersAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(string id)
        {
            var result = await _supplierlService.GetSupplierByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSupplierAsync([FromForm] SupplierCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _supplierlService.CreateSupplierAsync(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSupplierAsync([FromForm] SupplierUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var result = await _supplierlService.UpdateSupplierAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplierAsync(string id)
        {
            var result = await _supplierlService.DeleteSupplier(id);
            return Ok(result);
        }
    }
}
