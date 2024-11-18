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
        public async Task<IActionResult> Index(string? searchString, string? trangThai, decimal? priceFrom, decimal? priceTo, string? productCode, string? size, string? connectionDistance, int? batteryCapacity, int? stockQuantity, string? brandCode, string? color, int page = 1,int pageSize = 5)
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
        // POST: SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,KichCo,DonGia,SoLuongTonKho,TrangThai,KieuKetNoi,KhoangCachKetNoi,DungLuongPin,MaKhuyenMai,MaHang,MaMauSac,HinhMinhHoa")] SanPham sanPham)
        {
            // Kiểm tra điều kiện không được để trống
            if (string.IsNullOrWhiteSpace(sanPham.MaSanPham))
            {
                ModelState.AddModelError("MaSanPham", "Mã sản phẩm không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(sanPham.TenSanPham))
            {
                ModelState.AddModelError("TenSanPham", "Tên sản phẩm không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(sanPham.KichCo))
            {
                ModelState.AddModelError("KichCo", "Kích cỡ không được để trống.");
            }

            if (sanPham.DonGia <= 0)
            {
                ModelState.AddModelError("DonGia", "Đơn giá phải lớn hơn 0.");
            }

            if (sanPham.SoLuongTonKho < 0)
            {
                ModelState.AddModelError("SoLuongTonKho", "Số lượng tồn kho không được âm.");
            }

            if (string.IsNullOrWhiteSpace(sanPham.TrangThai))
            {
                ModelState.AddModelError("TrangThai", "Trạng thái không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(sanPham.KieuKetNoi))
            {
                ModelState.AddModelError("KieuKetNoi", "Kiểu kết nối không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(sanPham.KhoangCachKetNoi))
            {
                ModelState.AddModelError("KhoangCachKetNoi", "Khoảng cách kết nối không được để trống.");
            }


            // Kiểm tra nếu DungLuongPin <= 0 hoặc có giá trị trống
            if (sanPham.DungLuongPin <= 0)
            {
                ModelState.AddModelError("DungLuongPin", "Dung lượng pin phải lớn hơn 0.");
            }
            else if (sanPham.DungLuongPin == null)
            {
                ModelState.AddModelError("DungLuongPin", "Dung lượng pin không được để trống.");
            }


            if (string.IsNullOrWhiteSpace(sanPham.HinhMinhHoa))
            {
                ModelState.AddModelError("HinhMinhHoa", "URL hình minh họa không được để trống.");
            }

            // Kiểm tra trùng mã sản phẩm
            var existingSanPham = await _context.SanPhams
                                                 .FirstOrDefaultAsync(sp => sp.MaSanPham == sanPham.MaSanPham);
            if (existingSanPham != null)
            {
                ModelState.AddModelError("MaSanPham", "Mã sản phẩm này đã tồn tại.");
            }

            // Kiểm tra nếu tất cả các điều kiện hợp lệ
            if (ModelState.IsValid)
            {
                // Thêm sản phẩm mới vào cơ sở dữ liệu
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, giữ lại các giá trị trong ViewData
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
        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSanPham,TenSanPham,KichCo,DonGia,SoLuongTonKho,TrangThai,KieuKetNoi,KhoangCachKetNoi,DungLuongPin,MaKhuyenMai,MaHang,MaMauSac,HinhMinhHoa")] SanPham sanPham)
        {
            if (id != sanPham.MaSanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin sản phẩm
                    // Nếu có thay đổi URL ảnh, thì giá trị HinhMinhHoa sẽ được cập nhật
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

            // Nếu có lỗi, giữ lại các giá trị trong ViewData
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
