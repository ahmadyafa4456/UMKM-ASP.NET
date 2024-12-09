﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMKM_C_.Data;
using UMKM_C_.Models;

namespace UMKM_C_.Controllers
{
    public class BahanController : Controller
    {
        private readonly ApplicationDbContext db;
        public BahanController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(string input)
        {
            List<Bahan> bahan = db.Bahan.ToList();
            if (!String.IsNullOrEmpty(input))
            {
                bahan = db.Bahan.Where(p => p.nama.Contains(input)).ToList();
            }
            return View(bahan);
        }

        public IActionResult Tambah()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            Bahan bahan = db.Bahan.Where(u => u.Id == id).FirstOrDefault();
            if(bahan == null)
            {
                return NotFound();
            }
            return View(bahan);
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(Bahan model)
        {
            if (ModelState.IsValid)
            {
                model.created_at = DateTime.Now.ToString("yyyy-M-dd");
                await db.Bahan.AddAsync(model);
                await db.SaveChangesAsync();
                TempData["SuccessCreate"] = "data berhasil dibuat";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Bahan bahan)
        {
            if(ModelState.IsValid)
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
            if(bahan == null)
            {
                return NotFound();
            }
            db.Bahan.Remove(bahan);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
