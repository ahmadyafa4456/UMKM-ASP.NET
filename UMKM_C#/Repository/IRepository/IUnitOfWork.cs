namespace UMKM_C_.IRepository.Repository
{
    public interface IUnitOfWork
    {
        IBahanRepsitory Bahan { get; }
        IPemasukanRepository Pemasukan { get; }
        IPengeluaranRepository Pengeluaran { get; }
        Task Save();
    }
}