using DuAn1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DuAn1.Controllers
{
    public class QuanLyThongKeController : Controller
    {
        private readonly Duan1Context _context;

        public QuanLyThongKeController(Duan1Context context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? startDate, DateTime? endDate, string period)
        {
            // Get sales data
            decimal dailySales = GetSalesByDate(DateTime.Today);
            decimal weeklySales = GetSalesByDateRange(DateTime.Now.AddDays(-7), DateTime.Now);
            decimal monthlySales = GetSalesByMonth(DateTime.Now.Month, DateTime.Now.Year);
            decimal yearlySales = GetSalesByYear(DateTime.Now.Year);

            // Initialize custom range sales
            decimal customRangeSales = 0;

            // Check if the period is custom and if dates are provided
            if (period == "custom" && startDate.HasValue && endDate.HasValue)
            {
                customRangeSales = GetSalesByDateRange(startDate.Value, endDate.Value);
            }

            // Pass data to the view
            ViewBag.DailySales = dailySales;
            ViewBag.WeeklySales = weeklySales;
            ViewBag.MonthlySales = monthlySales;
            ViewBag.YearlySales = yearlySales;
            ViewBag.CustomRangeSales = customRangeSales;

            var productSales = GetProductSales();

            // Pass product sales data to the view
            ViewBag.ProductSales = productSales;

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            // Lấy tổng số khách hàng
            int totalCustomers = _context.KhachHangs.Count();

            // Lấy tổng số khách hàng đang hoạt động
            int activeCustomers = _context.KhachHangs.Count(kh => kh.TrangThai == "Đang hoạt động");

            int nam = _context.KhachHangs.Count(kh => kh.GioiTinh == "Nam");
            int nu = _context.KhachHangs.Count(kh => kh.GioiTinh == "Nữ");
            // Lưu vào ViewBag
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.ActiveCustomers = activeCustomers;
            ViewBag.Nam = nam;
            ViewBag.Nu = nu;

            return View();
        }

        // Helper methods to get sales data
        private decimal GetSalesByDate(DateTime date) =>
            _context.HoaDons
                .Where(h => h.NgayTao.HasValue && h.NgayTao.Value.Date == date.Date)
                .Sum(h => h.ThanhTien ?? 0);

        private decimal GetSalesByDateRange(DateTime startDate, DateTime endDate) =>
            _context.HoaDons
                .Where(h => h.NgayTao.HasValue && h.NgayTao.Value >= startDate && h.NgayTao.Value <= endDate)
                .Sum(h => h.ThanhTien ?? 0);

        private decimal GetSalesByMonth(int month, int year) =>
            _context.HoaDons.Where(h => h.NgayTao.HasValue && h.NgayTao.Value.Month == month && h.NgayTao.Value.Year == year)
                .Sum(h => h.ThanhTien ?? 0);

        private decimal GetSalesByYear(int year) =>
            _context.HoaDons
                .Where(h => h.NgayTao.HasValue && h.NgayTao.Value.Year == year)
                .Sum(h => h.ThanhTien ?? 0);
        private List<ProductSalesDto> GetProductSales()
        {
            var productSales = from h in _context.HoaDonChiTiets
                               join sp in _context.SanPhams on h.MaSanPham equals sp.MaSanPham
                               group new { h, sp } by sp.TenSanPham into g // Nhóm theo tên sản phẩm
                               select new ProductSalesDto
                               {
                                   MaSanPham = g.FirstOrDefault().h.MaSanPham,
                                   TenSanPham = g.FirstOrDefault().sp.TenSanPham,// Lưu mã sản phẩm nếu cần
                                   SoLuong = g.Sum(x => x.h.SoLuong ?? 0), // Tính tổng số lượng
                                   DoanhThu = g.Sum(x => (x.h.SoLuong ?? 0) * (x.h.DonGia ?? 0)) // Tính tổng doanh thu
                               };

            return productSales.ToList();
        }
        // DTO for product sales data
        public class ProductSalesDto
        {
            public required string MaSanPham { get; set; } = null!;
            public string? TenSanPham { get; set; } // Thêm thuộc tính tên sản phẩm
            public int SoLuong { get; set; }
            public decimal DoanhThu { get; set; }
        }
    }
}