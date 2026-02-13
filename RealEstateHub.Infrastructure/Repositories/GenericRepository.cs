using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Infrastructure.Data;
using System.Linq.Expressions;

namespace RealEstateHub.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _Context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _Context = dbContext;
            _dbSet = _Context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var obj = await _dbSet.FindAsync(id);
            if (obj == null) throw new NotFoundException(typeof(T).Name, id);
            return obj;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var resultlist = await _dbSet.Where(predicate).ToListAsync();
            return resultlist;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

    }
}
