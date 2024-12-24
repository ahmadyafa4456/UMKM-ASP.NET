using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using UMKM_C_.Data;
using UMKM_C_.IRepository.Repository;
using UMKM_C_.Models;
using UMKM_C_.Models.ViewModels;
using UMKM_C_.Services;

namespace UMKM_C_.Controllers
{
    [Authorize]
    public class BahanController : Controller
    {
        private readonly IUnitOfWork bahanRepo;
        private readonly Generate generate;
        public BahanController(IUnitOfWork db, Generate generate)
        {
            bahanRepo = db;
            this.generate = generate;
        }

        public async Task<IActionResult> Index(string input, int pg = 1)
        {
            IQueryable<Bahan> bahan = bahanRepo.Bahan.GetAll();
            if (!string.IsNullOrEmpty(input))
            {
                bahan = bahan.Where(p => p.nama.Contains(input));
            }
            const int pageSize = 5;
            if (pg < 1) { pg = 1; }
            int recsCount = await bahan.CountAsync();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            List<Bahan> bahanQuery = await bahan.Skip(recSkip).Take(pageSize).ToListAsync();
            this.ViewBag.Pager = pager;
            return View(bahanQuery);
        }

        public IActionResult Tambah()
        {
            var ViewModel = new TambahBahanViewModel();
            ViewModel.Bahan.Add(new Bahan());
            return View(ViewModel);
        }

        public IActionResult Edit(int id)
        {
            var bahan = bahanRepo.Bahan.Get(p => p.Id == id);
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
                    await bahanRepo.Bahan.Add(bahan);
                }
                await bahanRepo.Save();
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
                bahanRepo.Bahan.Update(bahan);
                await bahanRepo.Save();
                TempData["SuccessEdit"] = "data berhasil diedit";
                return RedirectToAction("Index");
            }
            return View(bahan);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bahan = await bahanRepo.Bahan.Get(p => p.Id == id);
            if (bahan == null)
            {
                return NotFound();
            }
            bahanRepo.Bahan.Remove(bahan);
            await bahanRepo.Save();
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
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    IgnoreBlankLines = false,
                    Delimiter = ",",
                    DetectDelimiter = false,
                    TrimOptions = TrimOptions.Trim
                });
                var records = csv.GetRecords<Bahan>().ToList();
                foreach (var r in records)
                {
                    await bahanRepo.Bahan.Add(r);
                }
                await bahanRepo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("File", "Harap unggah file.");
            }
            return View();
        }

        public async Task<IActionResult> GeneratePdf()
        {
            IQueryable<Bahan> query = bahanRepo.Bahan.GetAll();
            List<Bahan> bahan = await query.ToListAsync();
            var document = generate.pdfBahan.GenerateBahan(bahan);
            return File(document, "Application/pdf", "bahan.pdf");
        }

        public async Task<IActionResult> GenerateExcel()
        {
            IQueryable<Bahan> query = bahanRepo.Bahan.GetAll();
            List<Bahan> bahan = await query.ToListAsync();
            var fileContent = generate.excelBahan.GenerateBahan(bahan);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BahanReport.xlsx");
        }
    }
}
