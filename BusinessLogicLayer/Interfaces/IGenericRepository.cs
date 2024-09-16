using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        void create(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity? Get(int id);
        void Update(TEntity entity);
    }
}
