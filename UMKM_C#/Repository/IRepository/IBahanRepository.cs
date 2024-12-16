using UMKM_C_.Models;

namespace UMKM_C_.IRepository.Repository
{
    public interface IBahanRepsitory : IRepository<Bahan>
    {
        void Update(Bahan obj);
    }
}