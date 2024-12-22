using System.Linq.Expressions;
using UMKM_C_.Models;

namespace UMKM_C_.IRepository.Repository
{
    public interface IPemasukanRepository
    {
        Task AddPemasukanHarian(Pemasukan_harian harian);
        Task AddPemasukanBulanan(int id);
        Task AddImportPemasukan(List<Pemasukan_harian> harian);
        Task<Pemasukan_harian> GetPemasukanHarian(Expression<Func<Pemasukan_harian, bool>> filter);
        IQueryable<Pemasukan_harian> GetAllPemasukanHarian();
        IQueryable<Pemasukan_bulanan> GetAllPemasukanBulanan();
        void Update(Pemasukan_harian obj);
    }
}