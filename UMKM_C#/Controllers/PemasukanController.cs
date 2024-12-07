using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMKM_C_.Data;
using UMKM_C_.Models;
using UMKM_C_.Models.ViewModels;

namespace UMKM_C_.Controllers
{
    public class PemasukanController : Controller
    {
        private readonly ApplicationDbContext db;
        public PemasukanController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var today = DateTime.Now.ToString(format: "yyyy-MM-dd");
            var pemasukan = db.Pemasukan_harian.Where(p => p.Created_at == today).FirstOrDefault();
            return View(pemasukan);
        }

        public IActionResult Tambah()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var pemasukan = db.Pemasukan_harian.Where(p => p.Id == id).FirstOrDefault();
            if(pemasukan == null)
            {
                return NotFound();
            }
            return View(pemasukan);
        }

        public IActionResult PemasukanBulanan(string date)
        {
            var month = DateTime.Now.Month;
            var pemasukan_bulanan = db.Pemasukan_bulanan
                .Include(p => p.Pemasukan_harian)
                .Where(p => p.Bulan == month)
                .ToList();
            var total = db.Pemasukan_bulanan
                .Include(p => p.Pemasukan_harian)
                .Where(p => p.Bulan == month)
                .Sum(p => p.Pemasukan_harian.Total);
            var pemasukan_harian = db.Pemasukan_harian.Where(p => p.Created_at == date).FirstOrDefault();
            var pemasukan = new PemasukanViewModel()
            {
                Pemasukan_bulanan = pemasukan_bulanan,
                Pemasukan_harian = pemasukan_harian,
                Total = total
            };
            return View(pemasukan);
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(Pemasukan_harian model)
        {
            if(ModelState.IsValid)
            {
                model.Created_at = DateTime.Now.ToString(format: "yyyy-MM-dd");
                await db.Pemasukan_harian.AddAsync(model);
                await db.AddAsync(model);
                await db.SaveChangesAsync();
                Pemasukan_bulanan pemasukanBulanan = new Pemasukan_bulanan();
                pemasukanBulanan.HarianId = model.Id;
                pemasukanBulanan.Bulan = DateTime.Now.Month;
                await db.AddAsync(pemasukanBulanan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pemasukan_harian model)
        {
            if (ModelState.IsValid)
            {
                db.Pemasukan_harian.Update(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPemasukanBulanan()
        {
            var month = DateTime.Now.Month;
            var pemasukanBulanan = await db.Pemasukan_bulanan.Where(p => p.Bulan == month).Select(p => new { p.Pemasukan_harian.Total, p.Pemasukan_harian.Created_at }).ToListAsync();
            var total = pemasukanBulanan.Sum(p => p.Total);
            return Json(new
            {
                data = pemasukanBulanan,
                totalKeseluruhan = total
            });
        }
    }
}
