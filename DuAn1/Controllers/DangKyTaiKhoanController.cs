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
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                var gioHangCount = _context.GioHangs.Count();
                var maGioHang = $"GH{gioHangCount + 1}";  // Tạo mã giỏ hàng tự động theo số lượng giỏ hàng hiện tại

                var gioHang = new GioHang
                {
                    MaGioHang = maGioHang,  // Mã giỏ hàng tạo tự động
                    MaKhachHang = khachHang.MaKhachHang,  // Liên kết giỏ hàng với khách hàng
                    NgayThem = DateTime.Now,  // Ngày thêm giỏ hàng
                    SoLoaiSanPham = 0  // Ban đầu giỏ hàng chưa có sản phẩm
                };

                // Thêm giỏ hàng vào cơ sở dữ liệu
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();  // Lưu giỏ hàng vào cơ sở dữ liệu
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
