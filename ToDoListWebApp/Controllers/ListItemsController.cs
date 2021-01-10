using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Controllers
{
    [Authorize]
    public class ListItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ListItems
        public async Task<IActionResult> Index()
        {
           return View(await _context.ListItem.ToListAsync());
        }

        // GET: ListItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listItem = await _context.ListItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listItem == null)
            {
                return NotFound();
            }

            return View(listItem);
        }

        [Authorize("UserOwnsList")]
        // GET: ListItems/Create
        public IActionResult Create(int toDoListId)
        {
            if (toDoListId == 0 )
            {
                return NotFound();
            }

            if (_context.ToDoList.Find(toDoListId) == null)
            {
                return NotFound();
            }

            //Creates new list item so that this item will have the correct ToDoListId
            ListItem newItem = new ListItem
            {
                ToDoListId = toDoListId,
                //Sets default limit date time in one day
                LimitDateTime = DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddDays(1)
               
        };
            
            return View(newItem);
        }

        // POST: ListItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,LimitDateTime,UpdateDateTime,IsChecked, ToDoListId")] ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                listItem.CreateDateTime = DateTime.Now;
                listItem.UpdateDateTime = DateTime.Now;
                _context.Add(listItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ToDoLists", new { toDoListId = listItem.ToDoListId });
            }
            return View(listItem);
        }


        [Authorize("UserOwnsTask")]
        // GET: ListItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listItem = await _context.ListItem.FindAsync(id);
            if (listItem == null)
            {
                return NotFound();
            }
            return View(listItem);
        }

        // POST: ListItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreateDateTime,LimitDateTime,UpdateDateTime,IsChecked, ToDoListId")] ListItem listItem)
        {
            if (id != listItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    listItem.UpdateDateTime = DateTime.Now;
                    _context.Update(listItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListItemExists(listItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "ToDoLists", new { toDoListId = listItem.ToDoListId });

            }

            return RedirectToAction("Details", "ToDoLists", new { toDoListId = listItem.ToDoListId });
        }

        [Authorize("UserOwnsTask")]
        // GET: ListItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listItem = await _context.ListItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listItem == null)
            {
                return NotFound();
            }

            return View(listItem);
        }

        // POST: ListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listItem = await _context.ListItem.FindAsync(id);
            _context.ListItem.Remove(listItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "ToDoLists", new { toDoListId = listItem.ToDoListId });
        }

        private bool ListItemExists(int id)
        {
            return _context.ListItem.Any(e => e.Id == id);
        }
    }
}
