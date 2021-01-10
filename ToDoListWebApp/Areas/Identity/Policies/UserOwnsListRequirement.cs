using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListWebApp.Areas.Identity.Policies
{
    public class UserOwnsListRequirement : IAuthorizationRequirement
    {
        //Creates Requirement for custom authorization policy
    }
}
