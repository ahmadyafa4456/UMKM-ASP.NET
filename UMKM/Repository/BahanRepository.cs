using UMKM.Data;
using UMKM.IRepository.Repository;
using UMKM.Models;

namespace UMKM.Repository
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

        public async Task AddBahan(IEnumerable<Bahan> data)
        {
            await _db.AddRangeAsync(data);
        }
    }
}