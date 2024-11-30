using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMKM_C_.Data;
using UMKM_C_.Models;
using UMKM_C_.Models.ViewModels;

namespace UMKM_C_.Controllers
{
    public class PengeluaranController : Controller
    {
        private readonly ApplicationDbContext db;
        public PengeluaranController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult PengeluaranBulanan()
        {
            return View();
        }

        public IActionResult Tambah()
        {
            var viewModel = new BahanViewModel();
            viewModel.bahan.Add(new Pengeluaran_harian());
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(BahanViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var bahan in model.bahan)
                {
                    Pengeluaran_bulanan pengeluaran = new Pengeluaran_bulanan();
                    bahan.Created_at = DateTime.Now.ToString(format: "yyyy-MM-dd");
                    await db.Pengeluaran_harian.AddAsync(bahan);
                    await db.SaveChangesAsync();

                    pengeluaran.HarianId = bahan.Id;
                    pengeluaran.Bulan = DateTime.Now.Month;
                    await db.Pengeluaran_bulanan.AddAsync(pengeluaran);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPengeluaranHarian(string date)
        {
            var today = DateTime.Now.ToString(format: "yyyy-MM-dd");
            var pengeluaranHarian = await db.Pengeluaran_bulanan.Where(
                p => p.Pengeluaran_harian.Created_at == today
                ).Select(ph => new { ph.Pengeluaran_harian.Nama, ph.Pengeluaran_harian.Harga, ph.Pengeluaran_harian.Jumlah, ph.Pengeluaran_harian.Created_at })
                .ToListAsync();
            if (date != null)
            {
                pengeluaranHarian = await db.Pengeluaran_bulanan.Where(
                p => p.Pengeluaran_harian.Created_at == date
                ).Select(ph => new { ph.Pengeluaran_harian.Nama, ph.Pengeluaran_harian.Harga, ph.Pengeluaran_harian.Jumlah, ph.Pengeluaran_harian.Created_at })
                .ToListAsync();
            }
            if (pengeluaranHarian == null)
            {
                return NoContent();
            }
            return Ok(pengeluaranHarian);
        }

        public async Task<IActionResult> GetPengeluaranBulanan()
        {
            var month = DateTime.Now.Month;
            var pengeluaranBulanan = await db.Pengeluaran_bulanan.Where(pb => pb.Bulan == month).Select(
                ph => new { ph.Pengeluaran_harian.Nama, ph.Pengeluaran_harian.Harga, ph.Pengeluaran_harian.Jumlah, ph.Pengeluaran_harian.Created_at }
                ).ToListAsync();
            if(pengeluaranBulanan == null)
            {
                return NoContent();
            }
            return Ok(pengeluaranBulanan);
        }
    }
}
