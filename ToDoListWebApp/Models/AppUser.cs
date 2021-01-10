using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public virtual ICollection<ToDoList> ToDoLists { get; set; }

    }
}
