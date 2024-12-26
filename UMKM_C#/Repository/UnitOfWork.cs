using UMKM.Data;
using UMKM.IRepository.Repository;
using UMKM.Models;

namespace UMKM.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
        public IBahanRepsitory Bahan { get; private set; }
        public IPemasukanRepository Pemasukan { get; private set; }
        public IPengeluaranRepository Pengeluaran { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            Bahan = new BahanRepository(db);
            Pemasukan = new PemasukanRepository(db);
            Pengeluaran = new PengeluaranRepository(db);
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
    }
}