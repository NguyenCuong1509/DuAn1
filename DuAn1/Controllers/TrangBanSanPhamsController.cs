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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
            var query = BaseQuery();
            var total = await query.CountAsync();
            var sanPhams = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
            ViewBag.CurrentPage = page;

            return View(sanPhams);
        }

        public async Task<IActionResult> CuaHang(string? searchString, string? trangThai, decimal? priceFrom, decimal? priceTo,
            string? productCode, string? size, string? connectionDistance, int? batteryCapacity,
            int? stockQuantity, string? brandCode, string? color, int page = 1, int pageSize = 9)
        {
            var query = BaseQuery();

            if (!string.IsNullOrEmpty(searchString)) query = query.Where(p => p.TenSanPham.Contains(searchString));
            if (!string.IsNullOrEmpty(productCode)) query = query.Where(p => p.MaSanPham.Contains(productCode));
            if (!string.IsNullOrEmpty(size)) query = query.Where(p => p.KichCo.Contains(size));
            if (!string.IsNullOrEmpty(connectionDistance)) query = query.Where(p => p.KhoangCachKetNoi.Contains(connectionDistance));
            if (batteryCapacity.HasValue) query = query.Where(p => p.DungLuongPin == batteryCapacity);
            if (stockQuantity.HasValue) query = query.Where(p => p.SoLuongTonKho == stockQuantity);
            if (!string.IsNullOrEmpty(brandCode)) query = query.Where(p => p.MaHang.Contains(brandCode));
            if (!string.IsNullOrEmpty(color)) query = query.Where(p => p.MaMauSac.Contains(color));
            if (!string.IsNullOrEmpty(trangThai)) query = query.Where(p => p.TrangThai == trangThai);
            if (priceFrom.HasValue) query = query.Where(p => p.DonGia >= priceFrom);
            if (priceTo.HasValue) query = query.Where(p => p.DonGia <= priceTo);

            var total = await query.CountAsync();
            var sanPhams = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Brands = await _context.Hangs.ToListAsync();
            ViewBag.Colors = await _context.MauSacs.ToListAsync();

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