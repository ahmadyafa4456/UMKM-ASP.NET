using UMKM_C_.Models;

namespace UMKM_C_.IRepository.Repository
{
    public interface IPengeluaranRepository : IRepository<Pengeluaran_harian>
    {
        void Update(Pengeluaran_harian obj);
        Task AddPengeluaran(List<Pengeluaran_harian> data);
        Task AddPengeluaranBulanan(int id);
        IQueryable<Pengeluaran_bulanan> GetPengeluaranBulanan();
        // Task AddImportPengeluaran(List<Pengeluaran_harian> data);

    }
}