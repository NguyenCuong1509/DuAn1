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
    public class TrangBanSanPhamsController : Controller
    {
        private readonly Duan1Context _context;

        public TrangBanSanPhamsController(Duan1Context context)
        {
            _context = context;
        }

        public IActionResult LienHe() => View();
        public IActionResult ThongTin() => View();

        private IQueryable<SanPham> BaseQuery() => _context.SanPhams
            .Include(s => s.MaHangNavigation)
            .Include(s => s.MaKhuyenMaiNavigation)
            .Include(s => s.MaMauSacNavigation)
            .Where(s => s.TrangThai == "Đang kinh doanh")
            .AsQueryable();

        // ✅ Hiển thị 9 sản phẩm bán chạy nhất (hoặc tất cả nếu < 9)
        public async Task<IActionResult> Index()
        {
            try
            {
                var query = BaseQuery();

                // Nếu có cột SoLuongDaBan, ưu tiên sắp xếp theo bán chạy nhất
                var sanPhams = await query
                    .OrderByDescending(x => x.SoLuongTonKho) // Nếu chưa có cột này, thay bằng DonGia hoặc SoLuongTonKho
                    .Take(9) // Lấy tối đa 9 sản phẩm
                    .ToListAsync();

                // Trả ra view, không phân trang
                ViewBag.TotalPages = 1;
                ViewBag.CurrentPage = 1;

                return View(sanPhams);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi truy vấn sản phẩm: " + ex.Message);
                ViewBag.ErrorMessage = "Không thể tải danh sách sản phẩm.";
                return View(new List<SanPham>());
            }
        }

        // ✅ Trang cửa hàng - tìm kiếm sản phẩm
        public async Task<IActionResult> CuaHang(
            string? searchString,
            string? trangThai,
            decimal? priceFrom,
            decimal? priceTo,
            string? productCode,
            string? size,
            string? connectionDistance,
            int? batteryCapacity,
            int? stockQuantity,
            string? brandCode,
            string? color)
        {
            var query = BaseQuery();

            // Điều kiện lọc
            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(p => p.TenSanPham.Contains(searchString));
            if (!string.IsNullOrEmpty(productCode))
                query = query.Where(p => p.MaSanPham.Contains(productCode));
            if (!string.IsNullOrEmpty(size))
                query = query.Where(p => p.KichCo.Contains(size));
            if (!string.IsNullOrEmpty(connectionDistance))
                query = query.Where(p => p.KhoangCachKetNoi.Contains(connectionDistance));
            if (batteryCapacity.HasValue)
                query = query.Where(p => p.DungLuongPin == batteryCapacity);
            if (stockQuantity.HasValue)
                query = query.Where(p => p.SoLuongTonKho == stockQuantity);
            if (!string.IsNullOrEmpty(brandCode))
                query = query.Where(p => p.MaHang.Contains(brandCode));
            if (!string.IsNullOrEmpty(color))
                query = query.Where(p => p.MaMauSac.Contains(color));
            if (!string.IsNullOrEmpty(trangThai))
                query = query.Where(p => p.TrangThai == trangThai);
            if (priceFrom.HasValue)
                query = query.Where(p => p.DonGia >= priceFrom);
            if (priceTo.HasValue)
                query = query.Where(p => p.DonGia <= priceTo);

            // Nếu < 9 sản phẩm thì trả hết, nếu > 9 thì chỉ lấy 9 sản phẩm bán chạy nhất
            var total = await query.CountAsync();
            var sanPhams = await query
                .OrderByDescending(p => p.SoLuongTonKho)
                .Take(9)
                .ToListAsync();

            // Dữ liệu lọc bổ sung (Brand, Color)
            ViewBag.Brands = await _context.Hangs.AsNoTracking().ToListAsync();
            ViewBag.Colors = await _context.MauSacs.AsNoTracking().ToListAsync();
            ViewBag.TotalFound = total;

            return View(sanPhams);
        }


        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var sanPham = await BaseQuery().FirstOrDefaultAsync(m => m.MaSanPham == id);
            return sanPham == null ? NotFound() : View(sanPham);
        }

        public IActionResult Create()
        {
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang");
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai");
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,HinhMinhHoa,KichCo,DonGia,SoLuongTonKho,TrangThai,KieuKetNoi,KhoangCachKetNoi,DungLuongPin,MaKhuyenMai,MaHang,MaMauSac,LanSuaGanNhat")] SanPham sanPham)
        {
            if (!ModelState.IsValid) return ViewWithSelectLists(sanPham);
            _context.Add(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private IActionResult ViewWithSelectLists(SanPham sanPham)
        {
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", sanPham.MaHang);
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", sanPham.MaMauSac);
            return View(sanPham);
        }

        private async Task<IActionResult> AddToCartInternal(string MaSanPham, int SoLuong, bool redirectToPrevious = false)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _context.KhachHangs.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("DangNhap", "DangNhap");

            var cart = await _context.GioHangs.FirstOrDefaultAsync(g => g.MaKhachHang == user.MaKhachHang);
            if (cart == null)
            {
                cart = new GioHang
                {
                    MaKhachHang = user.MaKhachHang,
                    NgayThem = DateTime.Now,
                    SoLoaiSanPham = 0
                };
                _context.GioHangs.Add(cart);
                await _context.SaveChangesAsync();
            }

            var item = await _context.SanPhamGioHangs
                .FirstOrDefaultAsync(sp => sp.MaGioHang == cart.MaGioHang && sp.MaSanPham == MaSanPham);

            if (item == null)
            {
                item = new SanPhamGioHang { MaGioHang = cart.MaGioHang, MaSanPham = MaSanPham, SoLuong = SoLuong };
                _context.SanPhamGioHangs.Add(item);
            }
            else
            {
                item.SoLuong += SoLuong;
                _context.SanPhamGioHangs.Update(item);
            }

            cart.SoLoaiSanPham += SoLuong;
            _context.GioHangs.Update(cart);
            await _context.SaveChangesAsync();

            if (redirectToPrevious)
            {
                var referer = Request.Headers["Referer"].ToString();
                return string.IsNullOrEmpty(referer) ? RedirectToAction("CuaHang") : Redirect(referer);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public Task<IActionResult> AddToCart(string MaSanPham, int SoLuong) => AddToCartInternal(MaSanPham, SoLuong);

        [HttpPost]
        public Task<IActionResult> AddToCartCuaHang(string MaSanPham, int SoLuong) => AddToCartInternal(MaSanPham, SoLuong, true);
    }
}