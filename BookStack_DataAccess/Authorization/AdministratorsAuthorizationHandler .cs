using System.Threading.Tasks;
using BookStack_DataAccess;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BookStack_DataAccess.Authorization
{
    public class AdministratorAuthorizationHandler
                    : AuthorizationHandler<OperationAuthorizationRequirement, IdentityUser>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                     IdentityUser resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }
            
            // Administrators can do anything.
            if (context.User.IsInRole(Constants.AdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}