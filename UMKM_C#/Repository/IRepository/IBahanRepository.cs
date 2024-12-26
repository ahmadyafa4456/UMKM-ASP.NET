using UMKM.Models;

namespace UMKM.IRepository.Repository
{
    public interface IBahanRepsitory : IRepository<Bahan>
    {
        void Update(Bahan obj);
    }
}