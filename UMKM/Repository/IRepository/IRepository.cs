using System.Linq.Expressions;

namespace UMKM.IRepository.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}