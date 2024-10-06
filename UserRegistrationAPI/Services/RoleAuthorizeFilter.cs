using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UserRegistrationAPI.Services;
using System.Linq;

namespace UserRegistrationAPI.Authorization
{
    public class RoleAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _requiredRole;
        private readonly IUserService _userService;
        private readonly ILogger<RoleAuthorizeFilter> _logger;

        public RoleAuthorizeFilter(string requiredRole, IUserService userService, ILogger<RoleAuthorizeFilter> logger)
        {
            _requiredRole = requiredRole;
            _userService = userService;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Try to find the user ID in the "nameidentifier" claim
            var userId = context.HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userId == null)
            {
                _logger.LogWarning("Authorization failed: User ID not found in token.");
                context.Result = new UnauthorizedResult();
                return;
            }

            _logger.LogInformation($"RoleAuthorizeFilter: User ID '{userId}' found. Checking roles...");

            // Fetch the user based on the userId
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning($"Authorization failed: No user found with ID '{userId}'.");
                context.Result = new ForbidResult();
                return;
            }

            // Check if the user has the required role
            if (!user.Roles.Contains(_requiredRole))
            {
                _logger.LogWarning($"Authorization failed: User '{userId}' does not have the required role '{_requiredRole}'.");
                context.Result = new ForbidResult();
            }
            else
            {
                _logger.LogInformation($"Authorization succeeded: User '{userId}' has the required role '{_requiredRole}'.");
            }
        }
    }
}
