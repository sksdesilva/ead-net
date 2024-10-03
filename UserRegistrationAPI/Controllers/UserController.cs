using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserRegistrationAPI.Models;
using UserRegistrationAPI.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // Create a new user
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(ApplicationUser user)
    {
        var newUser = await _userService.CreateUserAsync(user);
        return Ok(newUser);
    }

    // Assign a role to a user
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        var success = await _userService.AddRoleToUserAsync(userId, role);
        if (success) return Ok("Role assigned successfully.");
        return BadRequest("Failed to assign role.");
    }
}
