using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoListWebApp.Models;

namespace ToDoListWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUser { get; set; }

        public DbSet<ToDoList> ToDoList { get; set; }

        public DbSet<ListItem> ListItem { get; set; }
    }
}
