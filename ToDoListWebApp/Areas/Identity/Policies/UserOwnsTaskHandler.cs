using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Areas.Identity.Policies
{
    public class UserOwnsTaskHandler : AuthorizationHandler<UserOwnsTaskRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public UserOwnsTaskHandler(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOwnsTaskRequirement requirement)
        {
            AppUser user = await _userManager.GetUserAsync(context.User);

            if (context.Resource is HttpContext http)
            {
                int id = int.Parse(http.Request.Path.Value.Split('/').Last());
                try
                {
                    var listItem = await _dbContext.ListItem.Include(item => item.ToDoList.Owner).FirstAsync(item => item.Id == id);
                    if (listItem != null && listItem.ToDoList.Owner == user)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
                catch (InvalidOperationException) { 
                    //list does not exit so context should fail;
                }
                
            }
            context.Fail();

        }
    }
}
