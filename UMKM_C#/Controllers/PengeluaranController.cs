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
            var today = DateTime.Now.ToString(format: "yyyy-MM-dd");
            List<Pengeluaran_harian> pengeluaran_harian = db.Pengeluaran_harian.Where(p => p.Created_at == today).ToList();
            return View(pengeluaran_harian);
        }

        public IActionResult PengeluaranBulanan(string date, int pg = 1)
        {
            var month = DateTime.Now.Month;
            var pengeluaran_bulanan = db.Pengeluaran_bulanan
                .Include(p => p.Pengeluaran_harian)
                .Where(p => p.Bulan == month)
                .ToList();
            var pengeluaran_harian = db.Pengeluaran_harian
                .Where(p => p.Created_at == date)
                .ToList();
            const int pageSize = 5;
            if (pg < 1) { pg = 1; };
            int recsCount = pengeluaran_bulanan.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            if(date != null)
            {
                 recsCount = pengeluaran_harian.Count();
                 pager = new Pager(recsCount, pg, pageSize);
                 recSkip = (pg - 1) * pageSize;
            }
            var pengeluaran = new PengeluaranViewModel()
            {
                Pengeluaran_Bulanan = pengeluaran_bulanan.Skip(recSkip).Take(pager.PageSize).ToList(),
                Pengeluaran_Harian = pengeluaran_harian.Skip(recSkip).Take(pager.PageSize).ToList()
            };
            this.ViewBag.Pager = pager;
            return View(pengeluaran);
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
    }
}
