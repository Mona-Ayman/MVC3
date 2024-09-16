using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Range(0, 500)]
        public int Code { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Display(Name = "Created At")]
        public DateTime Date { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
