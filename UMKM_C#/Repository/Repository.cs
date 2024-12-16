using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UMKM_C_.Data;
using UMKM_C_.IRepository.Repository;

namespace UMKM_C_.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext _db)
        {
            db = _db;
            this.dbSet = db.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            return await query.Where(filter).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet.AsQueryable();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}