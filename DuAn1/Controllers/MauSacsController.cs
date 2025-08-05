using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DuAn1.Models;

namespace DuAn1.Controllers
{
    public class MauSacsController : Controller
    {
        private readonly Duan1Context _context;

        public MauSacsController(Duan1Context context)
        {
            _context = context;
        }

        // GET: MauSacs
        public async Task<IActionResult> Index()
        {
            return View(await _context.MauSacs.ToListAsync());
        }

        // GET: MauSacs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MauSacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMauSac,TenMauSac")] MauSac mauSac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mauSac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mauSac);
        }

        // GET: MauSacs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs.FindAsync(id);
            if (mauSac == null)
            {
                return NotFound();
            }
            return View(mauSac);
        }

        // POST: MauSacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaMauSac,TenMauSac")] MauSac mauSac)
        {
            if (id != mauSac.MaMauSac)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mauSac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MauSacExists(mauSac.MaMauSac))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mauSac);
        }

        // GET: MauSacs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mauSac = await _context.MauSacs
                .FirstOrDefaultAsync(m => m.MaMauSac == id);
            if (mauSac == null)
            {
                return NotFound();
            }

            return View(mauSac);
        }

        // POST: MauSacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mauSac = await _context.MauSacs.FindAsync(id);
            if (mauSac != null)
            {
                _context.MauSacs.Remove(mauSac);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MauSacExists(string id)
        {
            return _context.MauSacs.Any(e => e.MaMauSac == id);
        }
    }
}
