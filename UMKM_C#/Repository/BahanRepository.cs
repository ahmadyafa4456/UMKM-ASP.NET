using UMKM_C_.Data;
using UMKM_C_.IRepository.Repository;
using UMKM_C_.Models;

namespace UMKM_C_.Repository
{
    public class BahanRepository : Repository<Bahan>, IBahanRepsitory
    {
        private readonly ApplicationDbContext _db;
        public BahanRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Bahan obj)
        {
            _db.Update(obj);
        }
    }
}