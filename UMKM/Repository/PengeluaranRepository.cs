using System.Globalization;
using Microsoft.EntityFrameworkCore;
using UMKM.Data;
using UMKM.IRepository.Repository;
using UMKM.Models;
using UMKM.Models.ViewModels;

namespace UMKM.Repository
{
    public class PengeluaranRepository : Repository<Pengeluaran_harian>, IPengeluaranRepository
    {
        private readonly ApplicationDbContext db;
        internal DbSet<Pengeluaran_bulanan> dbSetBulanan;
        public PengeluaranRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
            dbSetBulanan = db.Set<Pengeluaran_bulanan>();
        }
        public void Update(Pengeluaran_harian obj)
        {
            db.Update(obj);
        }

        public async Task AddPengeluaranBulanan(IEnumerable<Pengeluaran_bulanan> data)
        {
            await dbSetBulanan.AddRangeAsync(data);
        }

        public async Task AddPengeluaran(List<Pengeluaran_harian> data)
        {
            try
            {
                await db.Database.BeginTransactionAsync();
                foreach (var i in data)
                {
                    i.Created_at = DateTime.Now.ToString(format: "yyyy-MM-dd");
                }
                await db.AddRangeAsync(data);
                await db.SaveChangesAsync();
                var bulanan = data.Select(i => new Pengeluaran_bulanan
                {
                    HarianId = i.Id,
                    Bulan = DateTime.UtcNow.ToString("MMMM", new CultureInfo("id-ID"))
                }).ToList();
                await AddPengeluaranBulanan(bulanan);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task AddImportPengeluaranBulanan(IEnumerable<Pengeluaran_bulanan> data)
        {
            await dbSetBulanan.AddRangeAsync(data);
        }

        public async Task AddImportPengeluaran(PengeluaranVM data)
        {
            try
            {
                await db.Database.BeginTransactionAsync();
                await db.AddRangeAsync(data.harian);
                await db.SaveChangesAsync();
                var bulanan = data.harian.Select((harian, index) => new Pengeluaran_bulanan
                {
                    HarianId = harian.Id,
                    Bulan = data.bulan[index]
                }).ToList();
                await AddImportPengeluaranBulanan(bulanan);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public IQueryable<Pengeluaran_bulanan> GetPengeluaranBulanan()
        {
            var month = DateTime.UtcNow.Month;
            return dbSetBulanan.Include(p => p.Pengeluaran_harian).AsQueryable();
        }
    }
}