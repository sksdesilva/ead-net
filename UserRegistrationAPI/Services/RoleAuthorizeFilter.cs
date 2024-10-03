using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using UserRegistrationAPI.Services;

namespace UserRegistrationAPI.Authorization
{
    public class RoleAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _requiredRole;
        private readonly IUserService _userService;

        public RoleAuthorizeFilter(string requiredRole, IUserService userService)
        {
            _requiredRole = requiredRole;
            _userService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Retrieve user ID from claims
            var userId = context.HttpContext.User.FindFirst("UserId")?.Value;

            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Get the user and check their roles
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null || !user.Roles.Contains(_requiredRole))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
