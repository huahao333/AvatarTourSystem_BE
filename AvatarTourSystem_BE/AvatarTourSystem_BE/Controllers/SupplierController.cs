using BusinessObjects.Models;
using BusinessObjects.ViewModels.Supplier;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace AvatarTourSystem_BE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierlService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierlService = supplierService;
        }

        [HttpGet("suppliers-active")]
        public async Task<IActionResult> GetListActiveSuppliersAsync()
        {
            var result = await _supplierlService.GetActiveSuppliersAsync();
            return Ok(result);
        }

        [HttpGet("suppliers")]
        public async Task<IActionResult> GetListSuppliersAsync()
        {
            var result = await _supplierlService.GetSuppliersAsync();
            return Ok(result);
        }

        [HttpGet("supplier/{id}")]
        public async Task<IActionResult> GetSupplierByIdAsync(string id)
        {
            var result = await _supplierlService.GetSupplierByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("supplier")]
        public async Task<IActionResult> CreateSupplierAsync(SupplierCreateModel createModel)
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

        [HttpPut("supplier")]
        public async Task<IActionResult> UpdateSupplierAsync(SupplierUpdateModel updateModel)
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

        [HttpDelete("supplier/{id}")]
        public async Task<IActionResult> DeleteSupplierAsync(string id)
        {
            var result = await _supplierlService.DeleteSupplier(id);
            return Ok(result);
        }
    }
}
