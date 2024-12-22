using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMKM_C_.Data;
using UMKM_C_.IRepository.Repository;
using UMKM_C_.Models;
using UMKM_C_.Models.ViewModels;

namespace UMKM_C_.Controllers
{
    public class PengeluaranController : Controller
    {
        private readonly IUnitOfWork pengeluaran;
        public PengeluaranController(IUnitOfWork db)
        {
            pengeluaran = db;
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
            if (date != null)
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
            var viewModel = new TambahPengeluaranViewModel();
            viewModel.bahan.Add(new Pengeluaran_harian());
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(List<Pengeluaran_harian> data)
        {
            if (ModelState.IsValid)
            {
                await pengeluaran.Pengeluaran.AddPengeluaran(data);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
