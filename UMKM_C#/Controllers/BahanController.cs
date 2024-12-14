using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using UMKM_C_.Data;
using UMKM_C_.Models;
using UMKM_C_.Models.ViewModels;
using UMKM_C_.Services;

namespace UMKM_C_.Controllers
{
    public class BahanController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly GeneratePdfBahan generatePdfBahan;
        private readonly GenerateExcelBahan generateExcelBahan;
        public BahanController(ApplicationDbContext db, GeneratePdfBahan generatePdfBahan, GenerateExcelBahan generateExcelBahan)
        {
            this.db = db;
            this.generatePdfBahan = generatePdfBahan;
            this.generateExcelBahan = generateExcelBahan;
        }

        public IActionResult Index(string input, int pg = 1)
        {
            List<Bahan> bahan = db.Bahan.ToList();
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = bahan.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            if (input != null)
            {
                bahan = bahan.Where(p => p.nama.Contains(input)).ToList();
                recsCount = bahan.Count();
                pager = new Pager(recsCount, pg, pageSize);
                recSkip = (pg - 1) * pageSize;
                bahan = bahan.Skip(recSkip).Take(pager.PageSize).ToList();
            }
            bahan = bahan.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(bahan);
        }

        public IActionResult Tambah()
        {
            var ViewModel = new TambahBahanViewModel();
            ViewModel.Bahan.Add(new Bahan());
            return View(ViewModel);
        }

        public IActionResult Edit(int id)
        {
            Bahan bahan = db.Bahan.Where(u => u.Id == id).FirstOrDefault();
            if (bahan == null)
            {
                return NotFound();
            }
            return View(bahan);
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(TambahBahanViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var bahan in model.Bahan)
                {
                    bahan.created_at = DateTime.Now.ToString("yyyy-M-dd");
                    await db.Bahan.AddAsync(bahan);
                    await db.SaveChangesAsync();
                }
                TempData["SuccessCreate"] = "data berhasil dibuat";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Bahan bahan)
        {
            if (ModelState.IsValid)
            {
                db.Bahan.Update(bahan);
                await db.SaveChangesAsync();
                TempData["SuccessEdit"] = "data berhasil diedit";
                return RedirectToAction("Index");
            }
            return View(bahan);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            Bahan bahan = await db.Bahan.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (bahan == null)
            {
                return NotFound();
            }
            db.Bahan.Remove(bahan);
            await db.SaveChangesAsync();
            return Ok();
        }

        public IActionResult ImportBahan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportBahan(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (file != null && file.Length > 0)
            {
                // ambil lokasi untuk menaruh filenya
                var UploadDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";
                // buat pengecekan file
                var AllowExtension = ".csv";
                // ambil nama file di request
                var FileExtension = Path.GetExtension(file.FileName).ToLower();
                // lakukan pengecekan
                if (!AllowExtension.Contains(FileExtension))
                {
                    ModelState.AddModelError("File", "File harus berformat .csv.");
                    return View();
                }
                // check apakah file ada atau tidak
                if (!Directory.Exists(UploadDirectory))
                {
                    Directory.CreateDirectory(UploadDirectory);
                }
                // gabungkan alamat file yang tertuju dengan file request
                var FilePath = Path.Combine(UploadDirectory, file.FileName);
                // buat file
                using(var Stream = new FileStream(FilePath, FileMode.Create))
                {
                    await file.CopyToAsync(Stream);
                }
                using var reader = new StreamReader(FilePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    IgnoreBlankLines = false
                });
                var records = csv.GetRecords<Bahan>().ToList();
                foreach(var r in records)
                {
                    await db.Bahan.AddAsync(r);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("File", "Harap unggah file.");
            }
            return View();
        }

        public IActionResult GeneratePdf()
        {
            List<Bahan> bahan = db.Bahan.ToList();
            var document = generatePdfBahan.GenerateBahan(bahan);
            return File(document, "Application/pdf", "bahan.pdf");
        }

        public async Task<IActionResult> GenerateExcel()
        {
            List<Bahan> bahan = await db.Bahan.ToListAsync();
            var fileContent = generateExcelBahan.GenerateBahan(bahan);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BahanReport.xlsx");
        }
    }
}
