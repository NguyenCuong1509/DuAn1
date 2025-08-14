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
    public class QuanLyController : Controller
    {
        private readonly Duan1Context _context;

        public QuanLyController(Duan1Context context)
        {
            _context = context;
        }

        // GET: QuanLy/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin người dùng từ session
            var username = HttpContext.Session.GetString("UsernameQl");

            // Truyền tên người dùng vào ViewBag
            ViewBag.Username = username;

            var quanLy = await _context.QuanLies
                .FirstOrDefaultAsync(m => m.MaQuanLy == id);
            if (quanLy == null)
            {
                return NotFound();
            }

            return View(quanLy);
        }


        // GET: QuanLy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuanLy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQuanLy,TenQuanLy,NgaySinh,DiaChi,GioiTinh,TrangThai,Username,Password")] QuanLy quanLy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quanLy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            return View(quanLy);
        }

        // GET: QuanLy/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quanLy = await _context.QuanLies.FindAsync(id);
            if (quanLy == null)
            {
                return NotFound();
            }
            return View(quanLy);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("MaQuanLy,TenQuanLy,NgaySinh,DiaChi,GioiTinh,TrangThai,Username,Password")] QuanLy quanLy)
        {
            if (id != quanLy.MaQuanLy) return NotFound();

            if (ModelState.IsValid)
            {
                var existingQuanLy = await _context.QuanLies.AsNoTracking().FirstOrDefaultAsync(x => x.MaQuanLy == id);
                if (existingQuanLy == null) return NotFound();

                // Nếu không nhập password mới, giữ nguyên password cũ
                if (string.IsNullOrWhiteSpace(quanLy.Password))
                {
                    quanLy.Password = existingQuanLy.Password;
                }

                try
                {
                    _context.Update(quanLy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.QuanLies.Any(e => e.MaQuanLy == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Details), new { id = quanLy.MaQuanLy });
            }
            return View(quanLy);
        }


        private bool QuanLyExists(string id)
        {
            return _context.QuanLies.Any(e => e.MaQuanLy == id);
        }
    }
}
