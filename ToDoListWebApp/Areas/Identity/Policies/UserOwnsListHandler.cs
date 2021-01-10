using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Areas.Identity.Policies
{
    public class UserOwnsListHandler : AuthorizationHandler<UserOwnsListRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public UserOwnsListHandler(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOwnsListRequirement requirement)
        {

            AppUser user = await _userManager.GetUserAsync(context.User);

            if (context.Resource is HttpContext http)
            {
                if (http.Request.Query.ContainsKey("ToDoListId"))
                {
                    var toDoListId = int.Parse(http.Request.Query["ToDoListId"].ToString());
                    var toDoList = await _dbContext.ToDoList.FindAsync(toDoListId);
                    if (toDoList != null && toDoList.Owner == user)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
            context.Fail();

        }
    }
}
