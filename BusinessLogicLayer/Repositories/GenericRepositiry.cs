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

        public TEntity? Get(int id) => _dbset.Find(id);

        public IEnumerable<TEntity> GetAll() => _dbset.ToList();

        public int create(TEntity entity)
        {
            _dbset.Add(entity);
            return _context.SaveChanges();
        }

        public int Update(TEntity entity)
        {
            _dbset.Update(entity);
            return _context.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            _dbset.Remove(entity);
            return _context.SaveChanges();
        }
    }
}
