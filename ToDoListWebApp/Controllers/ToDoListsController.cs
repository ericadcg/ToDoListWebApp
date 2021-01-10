using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Controllers
{
    [Authorize]
    public class ToDoListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ToDoListsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToDoLists
        public async Task<IActionResult> Index()
        {
            //Get Lists belonging to current user
            AppUser user = await _context.AppUser.Include(u => u.ToDoLists).FirstAsync(u => u.Id == _userManager.GetUserId(User));
            return View(user.ToDoLists);
        }

        [Authorize("UserOwnsList")]
        // GET: ToDoLists/Details/5
        public async Task<IActionResult> Details(int? toDoListId)
        {
            if (toDoListId == null)
            {
                return NotFound();
            }

            //Includes ListItems (tasks) to show on details
            var toDoList = await _context.ToDoList.Include(l => l.ListItems)
                .FirstOrDefaultAsync(m => m.Id == toDoListId);
            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        
        // GET: ToDoLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                toDoList.Owner= await _userManager.GetUserAsync(User);
                toDoList.CreateDateTime = DateTime.Now;
                _context.Add(toDoList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoList);
        }

        [Authorize("UserOwnsList")]
        // GET: ToDoLists/Edit/5
        public async Task<IActionResult> Edit(int? toDoListId)
        {
            if (toDoListId == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoList.FindAsync(toDoListId);
            if (toDoList == null)
            {
                return NotFound();
            }
            return View(toDoList);
        }

        // POST: ToDoLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int toDoListId, [Bind("Id,Name,Description,CreateDateTime")] ToDoList toDoList)
        {
            if (toDoListId != toDoList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoListExists(toDoList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDoList);
        }

        [Authorize("UserOwnsList")]
        // GET: ToDoLists/Delete/5
        public async Task<IActionResult> Delete(int? toDoListId)
        {
            if (toDoListId == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoList
                .FirstOrDefaultAsync(m => m.Id == toDoListId);
            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        // POST: ToDoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int toDoListId)
        {
            var toDoList = await _context.ToDoList.FindAsync(toDoListId);
            _context.ToDoList.Remove(toDoList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoListExists(int id)
        {
            return _context.ToDoList.Any(e => e.Id == id);
        }

        //Updates the IsChecked on a Task; used on DetailsView to update without editing
        public JsonResult UpdateStatus(int id, bool status)
        {
            ListItem listItem = _context.ListItem.Find(id);
            if(listItem != null)
            {
                listItem.IsChecked = status;
                listItem.UpdateDateTime = DateTime.Now;
                _context.Entry(listItem).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Json(true);

        }
    }
}
