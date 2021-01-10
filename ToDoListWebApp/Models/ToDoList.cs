using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListWebApp.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:d MMM yyyy h:mm tt}")]
        [Display(Name = "Creation Date")]
        public DateTime CreateDateTime { get; set; }

        public virtual AppUser Owner { get; set; }

        public virtual ICollection<ListItem> ListItems { get; set; }
    }
}
