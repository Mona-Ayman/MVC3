using DataAccessLayer.Models;

namespace BusinessLogicLayer.Repositories
{
    public interface IDepartmentRepository
    {
        int create(Department entity);
        int Delete(Department entity);
        IEnumerable<Department> GetAll();
        Department? GetDepartment(int id);
        int Update(Department entity);
    }
}