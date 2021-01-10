using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListWebApp.Models
{
    public class ListItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
       
        public string Description { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime LimitDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public bool IsChecked { get; set; }

        public virtual ToDoList ToDoList { get; set; }
    }
}
