// All user product crud functions will be done in this compenet

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserRegistrationAPI.Models;
using UserRegistrationAPI.Services;
using UserRegistrationAPI.Authorization;

namespace UserRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create")]
        [RoleAuthorize("Vendor")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _productService.CreateProductAsync(product);
            return Ok("Product created successfully");
        }

        [HttpPatch("update/{id}")]
        [RoleAuthorize("Vendor")]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            await _productService.UpdateProductAsync(id, product);
            return Ok("Product updated successfully");
        }

        [HttpGet("vendor/{vendorName}")]
        [RoleAuthorize("Vendor")]
        public async Task<IActionResult> GetProductsByVendor(string vendorName)
        {
            var products = await _productService.GetProductsByVendorAsync(vendorName);
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found for the specified vendor.");
            }
            return Ok(products);
        }

        [HttpDelete("delete/{id}")]
        [RoleAuthorize("Vendor")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Product deleted successfully");
        }

        [HttpPut("activate/{id}")]
        [RoleAuthorize("Administrator")]
        public async Task<IActionResult> ActivateProduct(string id)
        {
            await _productService.ActivateProductAsync(id);
            return Ok("Product activated");
        }

        [HttpPut("deactivate/{id}")]
        [RoleAuthorize("Administrator")]
        public async Task<IActionResult> DeactivateProduct(string id)
        {
            await _productService.DeactivateProductAsync(id);
            return Ok("Product deactivated");
        }
    }
}
