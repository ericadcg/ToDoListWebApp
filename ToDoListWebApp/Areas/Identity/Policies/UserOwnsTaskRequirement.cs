using Microsoft.AspNetCore.Authorization;

namespace ToDoListWebApp.Areas.Identity.Policies
{
    public class UserOwnsTaskRequirement : IAuthorizationRequirement
    {
        //Creates Requirement for custom authorization policy
    }
}
