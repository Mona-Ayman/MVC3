using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Task<IEnumerable<Employee>> GetAllAsync(string name);
        public Task<IEnumerable<Employee>> GetAllWithDepartmentsAsync();
    }
}
