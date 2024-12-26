using Microsoft.EntityFrameworkCore;
using UMKM.Data;
using UMKM.IRepository.Repository;
using UMKM.Models;

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

        public async Task AddPengeluaranBulanan(int id)
        {
            Pengeluaran_bulanan bulanan = new Pengeluaran_bulanan
            {
                HarianId = id,
                Bulan = DateTime.UtcNow.Month
            };
            await dbSetBulanan.AddAsync(bulanan);
            await db.SaveChangesAsync();
        }

        // public async Task AddImportPengeluaran(List<Pengeluaran_harian> data)
        // {
        //     try
        //     {
        //         await db.Database.BeginTransactionAsync();
        //         foreach(var i in data)
        //         {
        //             i.Created_at = DateTime.Now.ToString(format: "yyyy-MM-dd");
        //             await db.AddAsync(i);
        //             await db.SaveChangesAsync();
        //             await AddPengeluaranBulanan()
        //         }
        //     }
        // }

        public async Task AddPengeluaran(List<Pengeluaran_harian> data)
        {
            try
            {
                await db.Database.BeginTransactionAsync();
                foreach (var i in data)
                {
                    i.Created_at = DateTime.Now.ToString(format: "yyyy-MM-dd");
                    await db.AddAsync(i);
                    await db.SaveChangesAsync();
                    await AddPengeluaranBulanan(i.Id);
                }
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