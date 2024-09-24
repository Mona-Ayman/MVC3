using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class EmployeeRepository : GenericRepositiry<Employee>, IEmployeeRepository
    {
        private DataContext _context;

        public EmployeeRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(string name) =>await _dbset.Where(e => e.Name.ToLower().Contains( name.ToLower())).Include(e => e.Department).ToListAsync();
        public async Task<IEnumerable<Employee>> GetAllWithDepartmentsAsync() => await _dbset.Include(e => e.Department).Include(e=>e.Department).ToListAsync();
      
    }
}
