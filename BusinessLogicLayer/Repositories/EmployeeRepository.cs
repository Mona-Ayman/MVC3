using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
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

    //    public int create(Employee entity)
    //    {
    //        _context.Add(entity);
    //        return _context.SaveChanges();
    //    }

    //    public int Delete(Employee entity)
    //    {
    //        _context.Remove(entity);
    //        return _context.SaveChanges();
    //    }

       public IEnumerable<Employee> GetAll(string Address) => _dbset.Where(e => e.Address.ToLower() == Address.ToLower()).ToList();

    //    public IEnumerable<Employee> GetAll() => _context.Employees.ToList();

    //    public Employee? GetEmployee(int id) => _context.Employees.Find(id);

    //    public int Update(Employee entity)
    //    {
    //        _context.Update(entity);
    //        return _context.SaveChanges();
    //    }
    }
}
