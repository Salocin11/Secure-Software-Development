using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Philip.Authorization
{
    public class EditDeleteRequirement : IAuthorizationRequirement
    {
        public EditDeleteRequirement(string userID)
        {
            UserID = userID;
        }

        public string UserID { get; set; }
    }

    public class EditDeleteRequirementHandler : AuthorizationHandler<EditDeleteRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EditDeleteRequirement requirement)
        {
            if (context.User.Identity.Name == requirement.UserID)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask; // This class was for checking whether or not users are requesting edits/deletes on their own articles. Since idk where to invoke this, its not usable.
        }
    }
}
