using UserRegistrationAPI.Models;

namespace UserRegistrationAPI.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task<bool> AddRoleToUserAsync(string userId, string role);
        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<List<ApplicationUser>> GetAllUsersAsync();

         Task<ApplicationUser> GetUserByUsernameAsync(string username);
    }
}
