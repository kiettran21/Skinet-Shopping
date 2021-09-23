using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Core.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;
using API.Exttensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRoleAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string[] roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            this.roles = roles ?? Array.Empty<string>();
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous) return;

            // Get Service user manager to find user
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();

            // Find user by email
            var user = await userManager.FindUserByClaimsPrincipal(context.HttpContext.User);

            if (user == null || (roles.Length > 0 && !roles.Contains("Admin")))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}