using System.Runtime.InteropServices;
using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace efcoreApp.Controllers
{
    public class KursKayitController : Controller
    {
        public readonly DataContext _context;
        public KursKayitController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var kursKayitlari = await _context.KursKayitlari
            .Include(x => x.Ogrenci).Include(x => x.Kurs).ToListAsync();
            //KURSKAYITLARININ YANINDA OGRENCI VE KURSUDA YUKLE YANI JOIN ISLEMI
            return View(kursKayitlari);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Ogrenciler = new SelectList(await _context.Ogrenciler.ToListAsync(),"OgrenciId","AdSoyad");
            ViewBag.Kurslar = new SelectList(await _context.Kurslar.ToListAsync(),"KursId","Baslik");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursKayit model)
        {
            model.KayitTarihi = DateTime.Now;
            _context.KursKayitlari.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}