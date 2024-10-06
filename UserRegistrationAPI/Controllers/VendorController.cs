using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserRegistrationAPI.Authorization;
using UserRegistrationAPI.Services;

[Route("api/[controller]")]
[ApiController]
public class VendorController : ControllerBase
{
    private readonly IVendorService _vendorService;

    public VendorController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    // POST: api/Vendor/Create
    [HttpPost("Create")]
    [RoleAuthorize("Administrator")] // Only accessible to Admin
    public async Task<IActionResult> CreateVendor([FromBody] Vendor vendor)
    {
        await _vendorService.CreateVendorAsync(vendor);
        return Ok("Vendor created successfully");
    }

    // GET: api/Vendor/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVendorById(string id)
    {
        var vendor = await _vendorService.GetVendorByIdAsync(id);
        if (vendor == null) return NotFound();
        return Ok(vendor);
    }

    // POST: api/Vendor/AddReview
    [HttpPost("AddReview")]
    [RoleAuthorize("Customer")]  // Only accessible to customers
    public async Task<IActionResult> AddVendorReview(string vendorId, [FromBody] VendorReview review)
    {
        await _vendorService.AddVendorRankingAsync(vendorId, review);
        return Ok("Review added successfully");
    }

    // PUT: api/Vendor/UpdateComment
    [HttpPut("UpdateComment")]
    [Authorize(Roles = "Customer")]  // Only customers can update their comments
    public async Task<IActionResult> UpdateCustomerComment(string vendorId, string customerId, string newComment)
    {
        await _vendorService.UpdateCustomerCommentAsync(vendorId, customerId, newComment);
        return Ok("Comment updated successfully");
    }
}
