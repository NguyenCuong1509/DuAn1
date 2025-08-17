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
    public class DangKyTaiKhoanController : Controller
    {
        private readonly Duan1Context _context;

        public DangKyTaiKhoanController(Duan1Context context)
        {
            _context = context;
        }

        // GET: DangKyTaiKhoan
        public async Task<IActionResult> Index()
        {
            return View(await _context.KhachHangs.ToListAsync());
        }

        // GET: DangKyTaiKhoan/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: DangKyTaiKhoan/Create
        public IActionResult Create()
        {
            var count = _context.KhachHangs.Count();
            var maKhachHang = $"KH{count + 1}";  // Auto-generate MaKhachHang

            // Create a new KhachHang object with MaKhachHang populated
            var khachHang = new KhachHang
            {
                MaKhachHang = maKhachHang,
                 TrangThai = "Đang hoạt động"
            };

            // Pass the khachHang object to the view
            return View(khachHang);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("MaKhachHang,HoTen,Sdt,DiaChi,GioiTinh,TrangThai,Username,Password")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra username đã tồn tại trong KhachHang hoặc QuanLy
                bool usernameExists = await _context.KhachHangs.AnyAsync(k => k.Username == khachHang.Username)
                                      || await _context.QuanLies.AnyAsync(q => q.Username == khachHang.Username);

                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "⚠ Username đã tồn tại, vui lòng chọn username khác.");
                    return View(khachHang);
                }

                // Lưu khách hàng
                _context.Add(khachHang);
                await _context.SaveChangesAsync();

                // Tạo giỏ hàng cho khách hàng
                var gioHangCount = await _context.GioHangs.CountAsync();
                var maGioHang = $"GH{gioHangCount + 1}";

                var gioHang = new GioHang
                {
                    MaGioHang = maGioHang,
                    MaKhachHang = khachHang.MaKhachHang,
                    NgayThem = DateTime.Now,
                    SoLoaiSanPham = 0
                };

                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();

                return RedirectToAction("DangNhap", "DangNhap");
            }

            return View(khachHang);
        }

        // GET: DangKyTaiKhoan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: DangKyTaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKhachHang,HoTen,Sdt,DiaChi,GioiTinh,TrangThai,Username,Password")] KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKhachHang))
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
            return View(khachHang);
        }

        // GET: DangKyTaiKhoan/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: DangKyTaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.MaKhachHang == id);
        }
    }
}
