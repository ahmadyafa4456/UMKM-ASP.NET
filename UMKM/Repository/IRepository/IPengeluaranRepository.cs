using UMKM.Models;
using UMKM.Models.ViewModels;

namespace UMKM.IRepository.Repository
{
    public interface IPengeluaranRepository : IRepository<Pengeluaran_harian>
    {
        void Update(Pengeluaran_harian obj);
        Task AddPengeluaran(List<Pengeluaran_harian> data);
        Task AddPengeluaranBulanan(IEnumerable<Pengeluaran_bulanan> data);
        IQueryable<Pengeluaran_bulanan> GetPengeluaranBulanan();
        Task AddImportPengeluaran(PengeluaranVM data);
        Task AddImportPengeluaranBulanan(IEnumerable<Pengeluaran_bulanan> data);

    }
}