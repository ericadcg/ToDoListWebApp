using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebApp.Controllers;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebAppUnitTests.Controllers
{
    [TestClass]
    public class ToDoListsControllerTest
    {
        static ApplicationDbContext _dbContext;
        static UserManager<AppUser> _userManager;

        //Initializes the dbContext and user manager
        [ClassInitialize]
        public static void InitToDoListControllerTest(TestContext testContext)
        {
            _dbContext = TestInitHelper.CreateTestingDbContext();
            _userManager = TestInitHelper.CreateTestingUserManager(_dbContext);
        }
        //Test Index View of ToDoLists
        [TestMethod]
        public void TestIndexView()
        {
            //gets a user
            var currUser = _dbContext.AppUser.Find("1");

            var context = TestInitHelper.CreateTestingControllerContext(currUser);

            ToDoListsController controller = new ToDoListsController(_dbContext, _userManager);
            controller.ControllerContext = context;

            var result = controller.Index().Result as ViewResult;

            if (result.Model is List<ToDoList> indexList)
            {
                //Gets count of ToDoLists belonging to current user in database
                //Gets count of ToDoLists of Index View
                //Compares counts
                Assert.AreEqual(_dbContext.ToDoList.Include(list => list.Owner).Where(list => list.Owner == currUser).Count(), indexList.Count());
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
