using System.Linq.Expressions;

namespace Restaurant.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        //methods
        // GetAll , GetById , Add , Remove , AddRange , RemoveRange
        Task <IEnumerable <T>>GetAll(Expression<Func<T, bool>>? filter =null ,
            string? includeProperties=null,
            Func<IQueryable<T> , IOrderedQueryable<T>>? OrderBy = null);
        Task<T> GetById(Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);
        Task add(T entity);
        Task addRange(IEnumerable<T> entities);
        void remove(T entity);
        void removeRange(IEnumerable<T> entities);
            
    }
}
