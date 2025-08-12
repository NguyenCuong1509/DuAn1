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
    public class QuanLyHoaDonController : Controller
    {
        private readonly Duan1Context _context;

        public QuanLyHoaDonController(Duan1Context context)
        {
            _context = context;
        }

        // GET: QuanLyHoaDon

        public async Task<IActionResult> Index(string? searchString, string? trangThai, decimal? priceFrom, decimal? priceTo, int page = 1, int pageSize = 10)
        {
            var hoaDonsQuery = _context.HoaDons.AsQueryable();

            // Áp dụng bộ lọc tìm kiếm theo mã hóa đơn nếu có
            if (!string.IsNullOrEmpty(searchString))
            {
                hoaDonsQuery = hoaDonsQuery.Where(h => h.MaHoaDon.Contains(searchString));
            }

            // Áp dụng bộ lọc trạng thái nếu có
            if (!string.IsNullOrEmpty(trangThai))
            {
                hoaDonsQuery = hoaDonsQuery.Where(h => h.TrangThai == trangThai);
            }

            // Áp dụng bộ lọc phạm vi giá nếu được cung cấp
            if (priceFrom.HasValue)
            {
                hoaDonsQuery = hoaDonsQuery.Where(h => h.ThanhTien >= priceFrom.Value);
            }
            if (priceTo.HasValue)
            {
                hoaDonsQuery = hoaDonsQuery.Where(h => h.ThanhTien <= priceTo.Value);
            }

            // Sắp xếp hóa đơn theo thời gian tạo từ mới nhất đến cũ nhất
            hoaDonsQuery = hoaDonsQuery.OrderByDescending(h => h.NgayTao); // Thay đổi ở đây

            // Thực hiện query và lấy danh sách hóa đơn với phân trang
            var totalRecords = await hoaDonsQuery.CountAsync();
            var hoaDons = await hoaDonsQuery
                .Include(h => h.MaGioHangNavigation) // Nếu cần thông tin giỏ hàng
                .Include(h => h.MaKhachHangNavigation) // Đảm bảo bạn đã bao gồm thông tin giỏ hàng
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Đặt thông tin phân trang vào ViewBag hoặc Model
            ViewBag.TotalRecords = totalRecords;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(hoaDons);
        }


        // GET: QuanLyHoaDon/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaGioHangNavigation) // Đảm bảo bạn đã bao gồm thông tin giỏ hàng
                 .Include(h => h.MaKhachHangNavigation) // Đảm bảo bạn đã bao gồm thông tin giỏ hàng
                .Include(h => h.HoaDonChiTiets) // Bao gồm chi tiết hóa đơn
                    .ThenInclude(d => d.MaSanPhamNavigation) // Bao gồm thông tin sản phẩm
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }
     
        // POST: QuanLyHoaDon/ChangeStatus
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string id, string newStatus)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newStatus))
            {
                return BadRequest("ID hoặc trạng thái không hợp lệ.");
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.HoaDonChiTiets) // Bao gồm chi tiết hóa đơn
                    .ThenInclude(d => d.MaSanPhamNavigation) // Bao gồm thông tin sản phẩm
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            // Kiểm tra và thay đổi trạng thái dựa trên quy tắc
            switch (hoaDon.TrangThai)
            {
                case "Chờ xác nhận":
                    if (newStatus == "Bắt đầu giao")
                    {
                        hoaDon.TrangThai = newStatus;

                        // Trừ số lượng sản phẩm khi bắt đầu giao
                        foreach (var item in hoaDon.HoaDonChiTiets)
                        {
                            var sanPham = await _context.SanPhams.FindAsync(item.MaSanPham);
                            if (sanPham != null)
                            {
                                sanPham.SoLuongTonKho -= item.SoLuong; // Giảm số lượng sản phẩm trong kho
                            }
                        }
                    }
                    else if (newStatus == "Hủy")
                    {
                        hoaDon.TrangThai = newStatus;
                    }
                    break;

                case "Bắt đầu giao":
                    if (newStatus == "Giao thành công")
                    {
                        hoaDon.TrangThai = newStatus;
                    }
                    break;

                case "Giao thành công":
                    if (newStatus == "Hủy")
                    {
                        hoaDon.TrangThai = newStatus;

                        // Thêm lại số lượng sản phẩm khi hủy đơn hàng
                        foreach (var item in hoaDon.HoaDonChiTiets)
                        {
                            var sanPham = await _context.SanPhams.FindAsync(item.MaSanPham);
                            if (sanPham != null)
                            {
                                sanPham.SoLuongTonKho += item.SoLuong; // Thêm lại số lượng sản phẩm vào kho
                            }
                        }
                    }
                    break;

                default:
                    return BadRequest("Trạng thái không hợp lệ cho việc thay đổi.");
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = hoaDon.MaHoaDon }); // Chuyển hướng về chi tiết hóa đơn
        }



    }
}
