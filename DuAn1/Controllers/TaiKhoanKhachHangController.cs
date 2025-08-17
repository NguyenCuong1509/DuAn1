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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.HoaDons)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // GET: TaiKhoanKhachHang/Edit/5 (Tùy chọn, có thể bỏ nếu chỉ dùng modal)
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
            return Json(khachHang); // Trả về JSON thay vì View nếu cần
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKhachHang,HoTen,Sdt,DiaChi,GioiTinh,Username,Password")] KhachHang updatedKhachHang)
        {
            System.Diagnostics.Debug.WriteLine($"Edit POST called with id: {id}, MaKhachHang: {updatedKhachHang.MaKhachHang}");
            if (string.IsNullOrEmpty(id) || id != updatedKhachHang.MaKhachHang)
            {
                return Json(new { success = false, message = $"ID không khớp: id={id}, MaKhachHang={updatedKhachHang.MaKhachHang}" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var khachHang = await _context.KhachHangs.FindAsync(id);
                    if (khachHang == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy khách hàng" });
                    }

                    // Cập nhật các trường từ form, bao gồm Username và Password
                    khachHang.HoTen = updatedKhachHang.HoTen;
                    khachHang.Sdt = updatedKhachHang.Sdt;
                    khachHang.DiaChi = updatedKhachHang.DiaChi;
                    khachHang.GioiTinh = updatedKhachHang.GioiTinh;
                    khachHang.Username = updatedKhachHang.Username;
                    khachHang.Password = updatedKhachHang.Password; // Lưu ý: Có thể cần mã hóa password trước khi lưu

                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("Username", khachHang.Username);
                    return Json(new { success = true, redirectUrl = Url.Action(nameof(Details), new { id = khachHang.MaKhachHang }) });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.KhachHangs.Any(e => e.MaKhachHang == updatedKhachHang.MaKhachHang))
                    {
                        return Json(new { success = false, message = "Không tìm thấy khách hàng" });
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                    return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            System.Diagnostics.Debug.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
            return Json(new { success = false, message = string.Join(", ", errors) });
        }
      public async Task<IActionResult> HoaDonChiTiet(string maHoaDon)
{
    if (maHoaDon == null) return NotFound();

    var chiTiets = await _context.HoaDonChiTiets
        .Include(h => h.MaSanPhamNavigation)
        .Where(h => h.MaHoaDon == maHoaDon)
        .ToListAsync();

    var hoaDon = await _context.HoaDons
        .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);

    if (chiTiets == null || !chiTiets.Any() || hoaDon == null) return NotFound();

    var model = new HoaDonDetailViewModel
    {
        MaHoaDon = hoaDon.MaHoaDon,
        MaKhachHang = hoaDon.MaKhachHang,
        TrangThai = hoaDon.TrangThai,
        ChiTiets = chiTiets
    };

    return View(model);
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyHoaDon(string MaHoaDon)
        {
            if (string.IsNullOrEmpty(MaHoaDon))
                return RedirectToAction("Details");

            var hoaDon = await _context.HoaDons
                .Include(hd => hd.HoaDonChiTiets)
                .FirstOrDefaultAsync(hd => hd.MaHoaDon == MaHoaDon);

            if (hoaDon == null)
                return RedirectToAction("Details");

            // Chỉ cho hủy nếu trạng thái là "Chờ xác nhận"
            if (hoaDon.TrangThai == "Chờ xác nhận")
            {
                hoaDon.TrangThai = "Đã hủy";
                _context.Update(hoaDon);

                // Nếu muốn trả lại số lượng tồn kho
                foreach (var ct in hoaDon.HoaDonChiTiets)
                {
                    var sanPham = await _context.SanPhams.FindAsync(ct.MaSanPham);
                    if (sanPham != null)
                    {
                        sanPham.SoLuongTonKho += ct.SoLuong;
                        _context.Update(sanPham);
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = hoaDon.MaKhachHang });
        }

    }
    public class HoaDonDetailViewModel
    {
        public string MaHoaDon { get; set; } = "";
        public string MaKhachHang { get; set; } = "";
        public string TrangThai { get; set; } = "";
        public IEnumerable<HoaDonChiTiet> ChiTiets { get; set; } = new List<HoaDonChiTiet>();
    }

}
