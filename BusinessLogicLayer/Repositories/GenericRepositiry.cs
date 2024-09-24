using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Repositories
{
    public class GenericRepositiry<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext _context;
        protected DbSet<TEntity> _dbset;

        public GenericRepositiry(DataContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetAsync(int id) => await _dbset.FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbset.ToListAsync();

        public async Task createAsync(TEntity entity)
        {
           await _dbset.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbset.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
        }
    }
}
