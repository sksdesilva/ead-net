using UserRegistrationAPI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using BCrypt.Net;

namespace UserRegistrationAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserService(IMongoClient client, IOptions<MongoDbSettings> settings)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<ApplicationUser>("Users");
        }

        // Create new user with password hashing
       public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
{
    // Ensure plain password is not null or empty
    if (string.IsNullOrEmpty(user.Password))
    {
        throw new ArgumentException("Password cannot be null or empty.");
    }

    // Hash the user's plain password
    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
    
    // Clear the plain password from memory for security purposes
    user.Password = null;

    // Save the user with the hashed password
    await _users.InsertOneAsync(user);
    return user;
}

public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        // Assign a new role to a user
        public async Task<bool> AddRoleToUserAsync(string userId, string role)
        {
            var update = Builders<ApplicationUser>.Update.AddToSet(u => u.Roles, role);
            var result = await _users.UpdateOneAsync(u => u.Id == userId, update);
            return result.ModifiedCount > 0;
        }

        // Get a user by ID
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _users.Find(u => true).ToListAsync(); // Returns all users
        }


        // Password hashing using BCrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
