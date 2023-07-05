using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GenericRepositoryAndUnitofWork.Filters
{
    public class AuthorizeFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeFilterAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity!.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            var authorized = _roles.Any(role => userRoles.Contains(role));

            if (!authorized)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
