using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        int create(TEntity entity);
        int Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity? Get(int id);
        int Update(TEntity entity);
    }
}
