using System.IO.Compression;
using System.Threading.Tasks;
using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileProviders;

namespace efcoreApp.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _context;
        public KursController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kurslar = await _context.Kurslar.Include(k => k.Ogretmen).ToListAsync();
            return View(kurslar);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursViewModel kurs)
        {
            if(ModelState.IsValid)
            {
                _context.Kurslar.Add(new Kurs() { KursId = kurs.KursId, Baslik = kurs.Baslik, OgretmenId = kurs.OgretmenId});
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","Ad");
            return View(kurs);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
                return NotFound();
            var kurs = await _context.Kurslar
            .Include(x => x.KursKayitlari)
            .ThenInclude(x => x.Ogrenci)
            .Select(k => new KursViewModel
            {
                KursId = k.KursId,
                Baslik = k.Baslik,
                OgretmenId = k.OgretmenId,
                KursKayitlari = k.KursKayitlari
            }).
            FirstOrDefaultAsync(x => x.KursId == id);

            if(kurs == null)
                return NotFound();

            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","Ad");

            return View(kurs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursViewModel kurs)
        {
            if(id != kurs.KursId)
                return NotFound();
            
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Kurs() { KursId = kurs.KursId, Baslik = kurs.Baslik, OgretmenId = kurs.OgretmenId});
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!_context.Kurslar.Any(k => k.KursId == kurs.KursId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","Ad");
            return View(kurs);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
                return NotFound();
            
            var kurs = await _context.Kurslar.FindAsync(id);
            if(kurs == null)
                return NotFound();
            
            return View(kurs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)
        {  
            var kurs = await _context.Kurslar.FindAsync(id);
            if(kurs == null)
                return NotFound();
            
            _context.Kurslar.Remove(kurs);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}