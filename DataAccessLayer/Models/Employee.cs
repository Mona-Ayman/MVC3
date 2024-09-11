using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Name { get; set; }
        [Range(20, 60)]
        public int Age { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }





    }
}
