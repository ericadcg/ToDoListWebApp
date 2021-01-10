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

        [DisplayFormat(DataFormatString = "{0:d MMM yyyy h:mm tt}")]
        [Display(Name = "Creation Date")]
        public DateTime CreateDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:d MMM yyyy h:mm tt}")]
        [Display(Name = "Limit Date")]
        public DateTime LimitDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:d MMM yyyy h:mm tt}")]
        [Display(Name = "Update Date")]
        public DateTime UpdateDateTime { get; set; }

        [Display(Name = "Is Done?")]
        public bool IsChecked { get; set; }

        public int ToDoListId { get; set; }

        public virtual ToDoList ToDoList { get; set; }
    }
}
