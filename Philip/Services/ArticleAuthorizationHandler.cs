using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Philip.Models;
using Microsoft.AspNetCore.Authorization;

namespace Philip.Services
{
    #region snippet_HandlerAndRequirement
    public class ArticleAuthorizationHandler :
        AuthorizationHandler<SameAuthorRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                        SameAuthorRequirement requirement,
                                                        Article resource)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if (context.User.Identity.Name == resource.Author || context.User.IsInRole("Admin"))
                {
                    context.Succeed(requirement);
                }
            }
            Console.Error.WriteLine(context.User.Identity.Name == resource.Author || context.User.IsInRole("Admin"));
            return Task.CompletedTask;
        }
    }

    public class SameAuthorRequirement : IAuthorizationRequirement { }
    #endregion

}
