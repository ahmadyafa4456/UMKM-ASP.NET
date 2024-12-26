using System.Globalization;
using System.Text;
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
    public class PemasukanController : Controller
    {
        private readonly IUnitOfWork pemasukanRepo;
        private readonly Generate generate;
        public PemasukanController(IUnitOfWork db, Generate generate)
        {
            pemasukanRepo = db;
            this.generate = generate;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Now.ToString(format: "yyyy-MM-dd");
            var pemasukan = await pemasukanRepo.Pemasukan.GetPemasukanHarian(p => p.Created_at == today);
            return View(pemasukan);
        }

        public IActionResult Tambah()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(Pemasukan_harian model)
        {
            if (ModelState.IsValid)
            {
                await pemasukanRepo.Pemasukan.AddPemasukanHarian(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pemasukan = await pemasukanRepo.Pemasukan.GetPemasukanHarian(p => p.Id == id);
            if (pemasukan == null)
            {
                return NotFound();
            }
            return View(pemasukan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pemasukan_harian model)
        {
            if (ModelState.IsValid)
            {
                pemasukanRepo.Pemasukan.Update(model);
                await pemasukanRepo.Save();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> PemasukanBulanan(string date, int pg = 1)
        {
            var month = DateTime.Now.Month;
            IQueryable<Pemasukan_bulanan> bulanan = pemasukanRepo.Pemasukan.GetAllPemasukanBulanan();
            if (!string.IsNullOrEmpty(date))
            {
                bulanan = bulanan.Where(p => p.Pemasukan_harian.Created_at == date);
            }
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = await bulanan.CountAsync();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            List<Pemasukan_bulanan> pemasukan_bulanan = await bulanan.Skip(recSkip).Take(pageSize).ToListAsync();
            var pemasukan = new PemasukanViewModel()
            {
                Pemasukan_bulanan = pemasukan_bulanan,
            };
            this.ViewBag.Pager = pager;
            return View(pemasukan);
        }

        [HttpGet]
        public async Task<IActionResult> GetPemasukanBulanan()
        {
            var month = DateTime.Now.Month;
            IQueryable<Pemasukan_bulanan> bulanan = pemasukanRepo.Pemasukan.GetAllPemasukanBulanan();
            var pemasukanBulanan = await bulanan.Where(p => p.Bulan == month).Select(p => new { p.Pemasukan_harian.Total, p.Pemasukan_harian.Created_at }).ToListAsync();
            var total = pemasukanBulanan.Sum(p => p.Total);
            return Json(new
            {
                data = pemasukanBulanan,
                totalKeseluruhan = total
            });
        }

        public async Task<IActionResult> GenerateExcel()
        {
            IQueryable<Pemasukan_bulanan> bulanan = pemasukanRepo.Pemasukan.GetAllPemasukanBulanan();
            List<Pemasukan_bulanan> data = await bulanan.ToListAsync();
            var file = generate.excelPemasukan.GeneratePemasukan(data);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PemasukanReport.xlsx");
        }

        public async Task<IActionResult> GeneratePdf()
        {
            IQueryable<Pemasukan_bulanan> bulanan = pemasukanRepo.Pemasukan.GetAllPemasukanBulanan();
            List<Pemasukan_bulanan> data = await bulanan.ToListAsync();
            var file = generate.pdfPemasukan.GeneratePemasukan(data);
            return File(file, "Application/pdf", "Pemasukan.pdf");
        }

        public IActionResult ImportPemasukan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportPemasukan(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                var record = csv.GetRecords<Pemasukan_harian>().ToList();
                await pemasukanRepo.Pemasukan.AddImportPemasukan(record);
                return RedirectToAction("PemasukanBulanan");
            }
            else
            {
                ModelState.AddModelError("File", "file tidak boleh kosong");
            }
            return View();
        }
    }
}
