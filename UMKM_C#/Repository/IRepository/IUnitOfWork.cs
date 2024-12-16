namespace UMKM_C_.IRepository.Repository
{
    public interface IUnitOfWork
    {
        IBahanRepsitory Bahan {get;}
        Task Save();
    }
}