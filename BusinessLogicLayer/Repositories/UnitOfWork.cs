
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Data;

namespace BusinessLogicLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IEmployeeRepository> _emprepo;
        private readonly Lazy<IDepartmentRepository> _deptrepo;
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _emprepo = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
            _deptrepo = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(context));
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
          return await  _context.SaveChangesAsync();
        }
        public IEmployeeRepository Employees => _emprepo.Value;
        public IDepartmentRepository Departments => _deptrepo.Value;

    }
}
