using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebAppUnitTests
{
    public class TestInitHelper
    {
        //Creates and seeds DB context for testing
        public static ApplicationDbContext CreateTestingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseInMemoryDatabase(databaseName: "TestingDatabase")
                              .Options;
            var dbContext = new ApplicationDbContext(options);
            SeedTestDatabase(dbContext);
            return dbContext;
        }

        //Simulates the user manager, getting the users from the created dbContext
        public static UserManager<AppUser> CreateTestingUserManager(ApplicationDbContext dbContext)
        {
            List<AppUser> userList = dbContext.AppUser.ToListAsync().Result;
            var store = new Mock<IUserStore<AppUser>>();
            var mockManager = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            mockManager.Object.UserValidators.Add(new UserValidator<AppUser>());
            mockManager.Object.PasswordValidators.Add(new PasswordValidator<AppUser>());

            mockManager.Setup(x => x.DeleteAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            mockManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<AppUser, string>((x, y) => userList.Add(x));
            mockManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            mockManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns<ClaimsPrincipal>(user => userList.Find(dbUser => dbUser.UserName == user.Identity.Name).Id);

            return mockManager.Object;
        }

        //Mocks project controller context with a specified user
        public static ControllerContext CreateTestingControllerContext(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("name", user.UserName),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);

            var projectControllerContext = new Mock<ControllerContext>();
            projectControllerContext.Object.HttpContext = mockHttpContext.Object;

            return projectControllerContext.Object;
        }

        //Adds Data to dbContext for testing
        public static void SeedTestDatabase(ApplicationDbContext dbContext)
        {

            List<AppUser> users = new List<AppUser>
            {
                new AppUser() { Id = "1", Email="user1@todo.com", UserName="user1@todo.com" },
                new AppUser() { Id = "2", Email="user2@todo.com", UserName="user2@todo.com" }
            };
            dbContext.AppUser.AddRange(users);

            List<ToDoList> testLists = new List<ToDoList>
            {
                new ToDoList(){Id=1, CreateDateTime=DateTime.Now, Description= "Test List for user 1", Name= "List 1", Owner=users.Find(u => u.Id=="1")},
                new ToDoList(){Id=2, CreateDateTime=DateTime.Now, Description= "Test List for user 1", Name= "List 2", Owner=users.Find(u => u.Id=="1")},
                new ToDoList(){Id=3, CreateDateTime=DateTime.Now, Description= "Test List for user 3", Name= "List 3", Owner=users.Find(u => u.Id=="2")}
            };

            dbContext.ToDoList.AddRange(testLists);
            dbContext.SaveChanges();
        }
    }
}
