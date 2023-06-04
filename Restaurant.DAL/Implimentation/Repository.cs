using Microsoft.EntityFrameworkCore;
using Restaurant.DAL.Interfaces;
using Restaurant.DAL.Migrations;
using System.Linq.Expressions;

namespace Restaurant.DAL.Implimentation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        public DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task add(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public async Task addRange(IEnumerable<T> entities)
        {
           await _dbSet.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, 
            string? includeProperties = null,Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //includeProperties = Category,FoodType
                string[] arrProprties = includeProperties.Split(",",StringSplitOptions.RemoveEmptyEntries);
                
                foreach (string prop in arrProprties)
                {
                    query = query.Include(prop);
                }
            }

            if (OrderBy != null)
            {
                return await OrderBy(query).ToListAsync();  
            }
            
            return await query.ToListAsync();
        }

      

        public async Task<T> GetById(Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //includeProperties = Category,FoodType
                string[] arrProprties = includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (string prop in arrProprties)
                {
                    query = query.Include(prop);
                }
            }
            return await query.FirstOrDefaultAsync();

          
        }

     
        public void remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void removeRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
