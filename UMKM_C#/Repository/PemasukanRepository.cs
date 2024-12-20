using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UMKM_C_.Data;
using UMKM_C_.IRepository.Repository;
using UMKM_C_.Models;

namespace UMKM_C_.Repository
{
    public class PemasukanRepository : IPemasukanRepository
    {
        private readonly ApplicationDbContext db;
        internal DbSet<Pemasukan_harian> dbSetHarian;
        internal DbSet<Pemasukan_bulanan> dbSetBulanan;
        public PemasukanRepository(ApplicationDbContext db)
        {
            this.db = db;
            this.dbSetHarian = db.Set<Pemasukan_harian>();
            this.dbSetBulanan = db.Set<Pemasukan_bulanan>();
        }

        public async Task AddPemasukanBulanan(int id)
        {
            var bulanan = new Pemasukan_bulanan
            {
                HarianId = id,
                Bulan = DateTime.UtcNow.Month
            };
            await dbSetBulanan.AddAsync(bulanan);
            await db.SaveChangesAsync();
        }

        public async Task AddPemasukanHarian(Pemasukan_harian harian)
        {
            try
            {
                await db.Database.BeginTransactionAsync();
                harian.Created_at = DateTime.Now.ToString(format: "yyyy-MM-dd");
                await dbSetHarian.AddAsync(harian);
                await db.SaveChangesAsync();
                await AddPemasukanBulanan(harian.Id);
                await db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public IQueryable<Pemasukan_bulanan> GetAllPemasukanBulanan()
        {
            var month = DateTime.Now.Month;
            return dbSetBulanan.Include(p => p.Pemasukan_harian).Where(p => p.Bulan == month).AsQueryable();
        }

        public IQueryable<Pemasukan_harian> GetAllPemasukanHarian()
        {
            return dbSetHarian.AsQueryable();
        }

        public async Task<Pemasukan_harian> GetPemasukanHarian(Expression<Func<Pemasukan_harian, bool>> filter)
        {
            IQueryable<Pemasukan_harian> query = dbSetHarian;
            return await query.Where(filter).FirstOrDefaultAsync();
        }

        public void Update(Pemasukan_harian obj)
        {
            db.Update(obj);
        }
    }
}