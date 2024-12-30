using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UMKM.IRepository.Repository;
using UMKM.Models;
using UMKM.Models.ViewModels;

namespace UMKM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork db;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork _db)
        {
            _logger = logger;
            db = _db;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<Bahan> bahan = db.Bahan.GetAll();
            IQueryable<Pemasukan_harian> pemasukan = db.Pemasukan.GetAllPemasukanHarian();
            IQueryable<Pengeluaran_harian> pengeluaran = db.Pengeluaran.GetAll();
            int TotalBahan = await bahan.CountAsync();
            int TotalPemasukan = pemasukan.Sum(p => p.Total);
            int TotalPengeluaran = pengeluaran.Sum(p => p.Harga);
            var data = new HomeVM
            {
                TotalBahan = TotalBahan,
                TotalPemasukan = TotalPemasukan,
                TotalPengeluaran = TotalPengeluaran
            };
            return View(data);
        }
    }
}
