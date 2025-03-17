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
        public IActionResult LienHe()
        {
            return View();
        }
        public IActionResult ThongTin()
        {
            return View();
        }

        // GET: TrangBanSanPhams
        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
            // Start the query with the SanPhams table including related entities
            var duan1Context = _context.SanPhams
         .Include(s => s.MaHangNavigation)
         .Include(s => s.MaKhuyenMaiNavigation)
         .Include(s => s.MaMauSacNavigation)
         .Where(s => s.TrangThai == "Đang kinh doanh") // Filter products with "Đang kinh doanh" status
         .AsQueryable(); // Ensure the query is still IQueryable for further filtering

            // Calculate total products count for pagination
            var totalProducts = await duan1Context.CountAsync();

            // Retrieve paged list of products
            var sanPhams = await duan1Context
                .Skip((page - 1) * pageSize) // Apply skip for pagination
                .Take(pageSize)              // Take the page size limit
                .ToListAsync();

            // Create ViewModel for pagination
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;

            // Return the paged data
            return View(sanPhams);
        }

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
     string? color,
     int page = 1,
     int pageSize = 9)
        {
            // Start the query with the SanPhams table including related entities
            var duan1Context = _context.SanPhams
         .Include(s => s.MaHangNavigation)
         .Include(s => s.MaKhuyenMaiNavigation)
         .Include(s => s.MaMauSacNavigation)
         .Where(s => s.TrangThai == "Đang kinh doanh") // Filter products with "Đang kinh doanh" status
         .AsQueryable(); // Ensure the query is still IQueryable for further filtering

            // Apply search filter if the search string is not null or empty
            if (!string.IsNullOrEmpty(searchString))
            {
                duan1Context = duan1Context.Where(p => p.TenSanPham.Contains(searchString));
            }

            // Apply other filters based on parameters
            if (!string.IsNullOrEmpty(productCode))
            {
                duan1Context = duan1Context.Where(p => p.MaSanPham.Contains(productCode));
            }
            if (!string.IsNullOrEmpty(size))
            {
                duan1Context = duan1Context.Where(p => p.KichCo.Contains(size));
            }
            if (!string.IsNullOrEmpty(connectionDistance))
            {
                duan1Context = duan1Context.Where(p => p.KhoangCachKetNoi.Contains(connectionDistance));
            }
            if (batteryCapacity.HasValue)
            {
                duan1Context = duan1Context.Where(p => p.DungLuongPin == batteryCapacity.Value);
            }
            if (stockQuantity.HasValue)
            {
                duan1Context = duan1Context.Where(p => p.SoLuongTonKho == stockQuantity.Value);
            }
            if (!string.IsNullOrEmpty(brandCode))
            {
                duan1Context = duan1Context.Where(p => p.MaHang.Contains(brandCode));
            }
            if (!string.IsNullOrEmpty(color))
            {
                duan1Context = duan1Context.Where(p => p.MaMauSac.Contains(color));
            }

            // Apply status filter if provided
            if (!string.IsNullOrEmpty(trangThai))
            {
                duan1Context = duan1Context.Where(p => p.TrangThai == trangThai);
            }

            // Apply price range filters if provided
            if (priceFrom.HasValue)
            {
                duan1Context = duan1Context.Where(p => p.DonGia >= priceFrom.Value);
            }
            if (priceTo.HasValue)
            {
                duan1Context = duan1Context.Where(p => p.DonGia <= priceTo.Value);
            }

            // Calculate total products count for pagination
            var totalProducts = await duan1Context.CountAsync();

            // Retrieve paged list of products
            var sanPhams = await duan1Context
                .Skip((page - 1) * pageSize) // Apply skip for pagination
                .Take(pageSize)              // Take the page size limit
                .ToListAsync();

            // Create ViewModel for pagination
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Brands = await _context.Hangs.ToListAsync();
            ViewBag.Colors = await _context.MauSacs.ToListAsync();

            return View(sanPhams);
        }


        // GET: TrangBanSanPhams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaHangNavigation)
                .Include(s => s.MaKhuyenMaiNavigation)
                .Include(s => s.MaMauSacNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: TrangBanSanPhams/Create
        public IActionResult Create()
        {
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang");
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai");
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac");
            return View();
        }

        // POST: TrangBanSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,HinhMinhHoa,KichCo,DonGia,SoLuongTonKho,TrangThai,KieuKetNoi,KhoangCachKetNoi,DungLuongPin,MaKhuyenMai,MaHang,MaMauSac,LanSuaGanNhat")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", sanPham.MaHang);
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", sanPham.MaMauSac);
            return View(sanPham);
        }

        // GET: TrangBanSanPhams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", sanPham.MaHang);
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", sanPham.MaMauSac);
            return View(sanPham);
        }

        // POST: TrangBanSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSanPham,TenSanPham,HinhMinhHoa,KichCo,DonGia,SoLuongTonKho,TrangThai,KieuKetNoi,KhoangCachKetNoi,DungLuongPin,MaKhuyenMai,MaHang,MaMauSac,LanSuaGanNhat")] SanPham sanPham)
        {
            if (id != sanPham.MaSanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSanPham))
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
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", sanPham.MaHang);
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", sanPham.MaMauSac);
            return View(sanPham);
        }

        // GET: TrangBanSanPhams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaHangNavigation)
                .Include(s => s.MaKhuyenMaiNavigation)
                .Include(s => s.MaMauSacNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: TrangBanSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(string id)
        {
            return _context.SanPhams.Any(e => e.MaSanPham == id);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public async Task<IActionResult> AddToCart(string MaSanPham, int SoLuong)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _context.KhachHangs.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

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

            var sanPhamGioHang = await _context.SanPhamGioHangs
                .FirstOrDefaultAsync(sp => sp.MaGioHang == cart.MaGioHang && sp.MaSanPham == MaSanPham);

            if (sanPhamGioHang == null)
            {
                sanPhamGioHang = new SanPhamGioHang
                {
                    MaGioHang = cart.MaGioHang,
                    MaSanPham = MaSanPham,
                    SoLuong = SoLuong
                };
                _context.SanPhamGioHangs.Add(sanPhamGioHang);
            }
            else
            {
                sanPhamGioHang.SoLuong += SoLuong;
                _context.SanPhamGioHangs.Update(sanPhamGioHang);
            }

            cart.SoLoaiSanPham += SoLuong;
            _context.GioHangs.Update(cart);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddToCartCuaHang(string MaSanPham, int SoLuong)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _context.KhachHangs.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

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

            var sanPhamGioHang = await _context.SanPhamGioHangs
                .FirstOrDefaultAsync(sp => sp.MaGioHang == cart.MaGioHang && sp.MaSanPham == MaSanPham);

            if (sanPhamGioHang == null)
            {
                sanPhamGioHang = new SanPhamGioHang
                {
                    MaGioHang = cart.MaGioHang,
                    MaSanPham = MaSanPham,
                    SoLuong = SoLuong
                };
                _context.SanPhamGioHangs.Add(sanPhamGioHang);
            }
            else
            {
                sanPhamGioHang.SoLuong += SoLuong;
                _context.SanPhamGioHangs.Update(sanPhamGioHang);
            }

            cart.SoLoaiSanPham += SoLuong;
            _context.GioHangs.Update(cart);

            await _context.SaveChangesAsync();
            // Redirect back to the page where the user was before
            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl); // Redirect to the previous page
            }
            else
            {
                return RedirectToAction("CuaHang"); // Default redirect if no referer is available
            }
        }
    }
}
