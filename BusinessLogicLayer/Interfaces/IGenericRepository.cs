using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        Task createAsync(TEntity entity);
        void Delete(TEntity entity);
		Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity>? GetAsync(int id);
        void Update(TEntity entity);
    }
}
