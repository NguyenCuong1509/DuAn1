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
    public class TaiKhoanKhachHangController : Controller
    {
        private readonly Duan1Context _context;

        public TaiKhoanKhachHangController(Duan1Context context)
        {
            _context = context;
        }
        // GET: TaiKhoanKhachHang/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.HoaDons) // Tải danh sách hóa đơn
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }


            return View(khachHang);
        }
        // GET: TaiKhoanKhachHang/Edit/5
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

        // POST: TaiKhoanKhachHang/Edit/5
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
                    if (!_context.KhachHangs.Any(e => e.MaKhachHang == khachHang.MaKhachHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = khachHang.MaKhachHang });
            }
            return View(khachHang);
        }

        // GET: TaiKhoanKhachHang/HoaDonChiTiet/5
        public async Task<IActionResult> HoaDonChiTiet(string maHoaDon)
        {
            if (maHoaDon == null)
            {
                return NotFound();
            }

            var hoaDonChiTiet = await _context.HoaDonChiTiets
                .Include(h => h.MaSanPhamNavigation) // Tải thông tin sản phẩm
                .Where(h => h.MaHoaDon == maHoaDon)
                .ToListAsync();

            if (hoaDonChiTiet == null || !hoaDonChiTiet.Any())
            {
                return NotFound();
            }

            return View(hoaDonChiTiet);
        }

    }
}
