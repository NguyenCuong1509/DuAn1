using DuAn1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DuAn1.Controllers
{
    public class QuanLySanPhamController : Controller
    {
		private readonly Duan1Context _context;

		public QuanLySanPhamController(Duan1Context context)
		{
			_context = context;
		}

        // GET: SanPhams
        public async Task<IActionResult> Index(string? searchString, string? trangThai, decimal? priceFrom, decimal? priceTo, string? productCode, string? size, string? connectionDistance, int? batteryCapacity, int? stockQuantity, string? brandCode, string? color)
        {
            var productsQuery = _context.SanPhams.AsQueryable();

            // Áp dụng bộ lọc tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.TenSanPham.Contains(searchString));
            }

            // Các bộ lọc mới
            if (!string.IsNullOrEmpty(productCode))
            {
                productsQuery = productsQuery.Where(p => p.MaSanPham.Contains(productCode));
            }
            if (!string.IsNullOrEmpty(size))
            {
                productsQuery = productsQuery.Where(p => p.KichCo.Contains(size));
            }
            if (!string.IsNullOrEmpty(connectionDistance))
            {
                productsQuery = productsQuery.Where(p => p.KhoangCachKetNoi.Contains(connectionDistance));
            }
            if (batteryCapacity.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.DungLuongPin == batteryCapacity.Value);
            }
            if (stockQuantity.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.SoLuongTonKho == stockQuantity.Value);
            }
            if (!string.IsNullOrEmpty(brandCode))
            {
                productsQuery = productsQuery.Where(p => p.MaHang.Contains(brandCode));
            }
            if (!string.IsNullOrEmpty(color))
            {
                productsQuery = productsQuery.Where(p => p.MaMauSac.Contains(color));
            }

            // Áp dụng bộ lọc trạng thái nếu có
            if (!string.IsNullOrEmpty(trangThai))
            {
                productsQuery = productsQuery.Where(p => p.TrangThai == trangThai);
            }

            // Áp dụng bộ lọc phạm vi giá nếu được cung cấp
            if (priceFrom.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.DonGia >= priceFrom.Value);
            }
            if (priceTo.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.DonGia <= priceTo.Value);
            }

            // Thực hiện query và lấy danh sách sản phẩm
            var sanPhams = await productsQuery
                .Include(s => s.MaHangNavigation)
                .Include(s => s.MaKhuyenMaiNavigation)
                .Include(s => s.MaMauSacNavigation)
                .ToListAsync();

            return View(sanPhams);
        }


        // GET: SanPhams/Details/5
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

		// GET: SanPhams/Create
		public IActionResult Create()
		{
			ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang");
			ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai");
			ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac");
			return View();
		}

		// POST: SanPhams/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham, HinhMinhHoa, KichCo,DonGia,SoLuongTonKho,TrangThai, KieuKetNoi, KhoangCachKetNoi, DungLuongPin, MaKhuyenMai,MaHang,MaMauSac")] SanPham sanPham)
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
        public async Task<IActionResult> UpdateStatus(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm sản phẩm theo ID
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái sản phẩm
            sanPham.TrangThai = "Ngừng kinh doanh";
            _context.Update(sanPham);
            await _context.SaveChangesAsync();

            // Trả về kết quả thành công
            return Json(new { success = true });
        }
        public async Task<IActionResult> UpdateTrangthai(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm sản phẩm theo ID
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái sản phẩm
            sanPham.TrangThai = "Đang kinh doanh";
            _context.Update(sanPham);
            await _context.SaveChangesAsync();

            // Trả về kết quả thành công
            return Json(new { success = true });
        }


        // GET: SanPhams/Edit/5
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

		// POST: SanPhams/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		// POST: SanPhams/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(string id, [Bind("MaSanPham,TenSanPham,KichCo,DonGia,SoLuongTonKho,TrangThai,MaKhuyenMai,MaHang,MaMauSac")] SanPham sanPham)
{
    if (id != sanPham.MaSanPham)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // Cập nhật trường "Lần Sửa Gần Nhất" với thời gian hiện tại
            sanPham.LanSuaGanNhat = DateTime.Now;

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

		// GET: SanPhams/Delete/5
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

		// POST: SanPhams/Delete/5
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
	}
}
