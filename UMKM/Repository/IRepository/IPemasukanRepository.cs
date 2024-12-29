using System.Linq.Expressions;
using UMKM.Models;
using UMKM.Models.ViewModels;

namespace UMKM.IRepository.Repository
{
    public interface IPemasukanRepository
    {
        Task AddPemasukanHarian(Pemasukan_harian harian);
        Task AddPemasukanBulanan(int id);
        Task AddImportPemasukan(PemasukanVM data);
        Task AddImportPemasukanBulanan(IEnumerable<Pemasukan_bulanan> data);
        Task<Pemasukan_harian> GetPemasukanHarian(Expression<Func<Pemasukan_harian, bool>> filter);
        IQueryable<Pemasukan_harian> GetAllPemasukanHarian();
        IQueryable<Pemasukan_bulanan> GetAllPemasukanBulanan();
        void Update(Pemasukan_harian obj);
    }
}