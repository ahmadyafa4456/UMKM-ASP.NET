using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMKM.Data;
using UMKM.IRepository.Repository;
using UMKM.Models;
using UMKM.Models.ViewModels;
using UMKM.Services;

namespace UMKM.Controllers
{
    [Authorize]
    public class PengeluaranController : Controller
    {
        private readonly IUnitOfWork db;
        private readonly Generate generate;
        public PengeluaranController(IUnitOfWork db, Generate generate)
        {
            this.db = db;
            this.generate = generate;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Now.ToString(format: "yyyy-MM-dd");
            IQueryable<Pengeluaran_harian> harian = db.Pengeluaran.GetAll();
            List<Pengeluaran_harian> pengeluaran_harian = await harian.Where(p => p.Created_at == today).ToListAsync();
            return View(pengeluaran_harian);
        }

        public async Task<IActionResult> PengeluaranBulanan(string date, int pg = 1)
        {
            IQueryable<Pengeluaran_bulanan> bulanan = db.Pengeluaran.GetPengeluaranBulanan();
            if (!string.IsNullOrEmpty(date))
            {
                bulanan = bulanan.Where(p => p.Pengeluaran_harian.Created_at == date);
            }
            const int pageSize = 5;
            if (pg < 1) { pg = 1; };
            int recsCount = await bulanan.CountAsync();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            List<Pengeluaran_bulanan> pengeluaran = await bulanan.Skip(recSkip).Take(pageSize).ToListAsync();
            this.ViewBag.Pager = pager;
            return View(pengeluaran);
        }

        public IActionResult Tambah()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(PengeluaranViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pengeluaran_harian = model.data.ToList();
                await db.Pengeluaran.AddPengeluaran(pengeluaran_harian);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult ImportPengeluaran()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportPengeluaran(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var allowExtension = ".csv";
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowExtension.Contains(fileExtension))
                {
                    ModelState.AddModelError("File", "file harus berformat csv");
                    return View();
                }
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    IgnoreBlankLines = false
                });
                var record = csv.GetRecords<ImportPengeluaran>().ToList();
                var data = new PengeluaranVM
                {
                    harian = record.Select(p => new Pengeluaran_harian { Nama = p.Nama, Jumlah = p.Jumlah, Harga = p.Harga, Created_at = DateTime.Now.ToString("yyyy-MM-dd") }).ToList(),
                    bulan = record.Select(p => p.Bulan).ToList()
                };
                await db.Pengeluaran.AddImportPengeluaran(data);
                return RedirectToAction("PengeluaranBulanan");
            }
            else
            {
                ModelState.AddModelError("File", "file tidak boleh kosong");
            }
            return View();
        }

        public async Task<IActionResult> GenerateExcel()
        {
            IQueryable<Pengeluaran_bulanan> data = db.Pengeluaran.GetPengeluaranBulanan();
            List<Pengeluaran_bulanan> bulanan = await data.ToListAsync();
            var file = generate.excelPengeluaran.GeneratePengeluaran(bulanan);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PengeluaranReport.xlsx");
        }

        public async Task<IActionResult> GeneratePdf()
        {
            IQueryable<Pengeluaran_bulanan> data = db.Pengeluaran.GetPengeluaranBulanan();
            List<Pengeluaran_bulanan> bulanan = await data.ToListAsync();
            var file = generate.pdfPengeluaran.GeneratePengeluaran(bulanan);
            return File(file, "Application/pdf", "Pengeluaran.pdf");
        }

        public async Task<IActionResult> DisplayData()
        {
            IQueryable<Pengeluaran_bulanan> data = db.Pengeluaran.GetPengeluaranBulanan();
            var pengeluaran = await data
                .GroupBy(p => p.Bulan)
                .Select(g => new
                {
                    Bulan = g.Key,
                    Total = g.Sum(p => p.Pengeluaran_harian.Harga)
                })
                .Distinct()
                .ToListAsync();
            return Ok(pengeluaran);
        }
    }
}
