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
        public async Task<IActionResult> Index(string? searchString, string? trangThai, decimal? priceFrom, decimal? priceTo, string? productCode, string? size, string? connectionDistance, int? batteryCapacity, int? stockQuantity, string? brandCode, string? color, int page = 1, int pageSize = 10)
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

            // Tính toán tổng số sản phẩm
            var totalProducts = await productsQuery.CountAsync();

            // Lấy danh sách sản phẩm theo trang
            var sanPhams = await productsQuery
                .Include(s => s.MaHangNavigation)
                .Include(s => s.MaKhuyenMaiNavigation)
                .Include(s => s.MaMauSacNavigation)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            sanPhams = sanPhams
     .OrderByDescending(k =>
     {
         if (k.MaSanPham != null && k.MaSanPham.StartsWith("KH"))
         {
             var partNumber = k.MaSanPham.Substring(2);
             return int.TryParse(partNumber, out var result) ? result : 0;
         }
         return 0;
     })
     .ToList();
            // Tạo ViewModel cho phân trang
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.Brands = await _context.Hangs.ToListAsync();
            ViewBag.Colors = await _context.MauSacs.ToListAsync();

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

            // Lấy danh sách mã khuyến mãi đang và sắp hoạt động
            var khuyenMais = await _context.KhuyenMais
        .Where(k => (k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now) || k.NgayBatDau > DateTime.Now)
        .ToListAsync();

            // Gửi dữ liệu cho View
            ViewData["KhuyenMais"] = new SelectList(khuyenMais, "MaKhuyenMai", "MaKhuyenMai");

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyPromoCode(string MaSanPham, string MaKhuyenMai)
        {
            // Kiểm tra xem mã sản phẩm và mã khuyến mãi có hợp lệ không
            if (string.IsNullOrEmpty(MaSanPham) || string.IsNullOrEmpty(MaKhuyenMai))
            {
                return BadRequest("Mã sản phẩm hoặc mã khuyến mãi không hợp lệ.");
            }

            var sanPham = await _context.SanPhams.FindAsync(MaSanPham);
            if (sanPham == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            var khuyenMai = await _context.KhuyenMais.FindAsync(MaKhuyenMai);
            if (khuyenMai == null)
            {
                return NotFound("Khuyến mãi không tồn tại.");
            }

            // Tạo chi tiết khuyến mãi
            var chiTietKhuyenMai = new ChiTietKhuyenMai
            {
                MaKhuyenMai = MaKhuyenMai,
                MaSanPham = MaSanPham,
                NgayBatDau = khuyenMai.NgayBatDau,
                NgayKetThuc = khuyenMai.NgayKetThuc,
                TrangThai = khuyenMai.TrangThai // Giả sử bạn muốn lấy trạng thái từ khuyến mãi
            };

            // Thêm chi tiết khuyến mãi vào cơ sở dữ liệu
            _context.ChiTietKhuyenMais.Add(chiTietKhuyenMai);
            await _context.SaveChangesAsync();

            // Cập nhật mã khuyến mãi cho sản phẩm
            sanPham.MaKhuyenMai = MaKhuyenMai;
            _context.Update(sanPham);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = MaSanPham });
        }


        public IActionResult Create()
        {
            // Lấy tổng số sản phẩm hiện tại
            int totalSanPham = _context.SanPhams.Count();

            // Tạo mã sản phẩm tiếp theo, giả sử mã sản phẩm có dạng "SP0001", "SP0002", ...
            string newMaSanPham = "SP" + (totalSanPham + 1).ToString("D2");

            // Lọc các mã khuyến mãi có trạng thái "Sắp bắt đầu" hoặc "Đang hoạt động"
            var khuyenMais = _context.KhuyenMais
                                     .Where(km => km.TrangThai == "Sắp bắt đầu" || km.TrangThai == "Đang hoạt động")
                                     .ToList();

            // Lọc các hãng có trạng thái "Đang hợp tác"
            var hangs = _context.Hangs
                                .Where(h => h.TrangThai == "Đang hợp tác")
                                .ToList();

            // Thêm lựa chọn "Không có mã khuyến mãi" vào danh sách mã khuyến mãi
            khuyenMais.Insert(0, new KhuyenMai { MaKhuyenMai = "", TenKhuyenMai = "Không có mã khuyến mãi" });

            // Gửi danh sách khuyến mãi vào ViewBag để sử dụng trong View
            ViewData["MaKhuyenMai"] = new SelectList(khuyenMais, "MaKhuyenMai", "TenKhuyenMai");

            // Gửi mã sản phẩm mới vào ViewData để có thể sử dụng trong View
            ViewData["NewMaSanPham"] = newMaSanPham;

            // Gửi danh sách hãng vào ViewData
            ViewData["MaHang"] = new SelectList(hangs, "MaHang", "TenHang");

            // Gửi danh sách màu sắc vào ViewData
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "TenMauSac");

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

            // Lọc các mã khuyến mãi có trạng thái "Sắp bắt đầu" hoặc "Đang hoạt động"
            var khuyenMais = _context.KhuyenMais
                                     .Where(km => km.TrangThai == "Sắp bắt đầu" || km.TrangThai == "Đang hoạt động")
                                     .ToList();

            // Thêm lựa chọn "Không có mã khuyến mãi"
            khuyenMais.Insert(0, new KhuyenMai { MaKhuyenMai = "", TenKhuyenMai = "Không có mã khuyến mãi" });

            // Lọc các hãng có trạng thái "Đang hợp tác"
            var hangs = _context.Hangs
                                .Where(h => h.TrangThai == "Đang hợp tác")
                                .ToList();

            // Gửi danh sách khuyến mãi vào ViewData để sử dụng trong View
            ViewData["MaKhuyenMai"] = new SelectList(khuyenMais, "MaKhuyenMai", "TenKhuyenMai", sanPham.MaKhuyenMai);

            // Gửi các danh sách khác như mã hàng, mã màu sắc vào ViewData
            ViewData["MaHang"] = new SelectList(hangs, "MaHang", "TenHang", sanPham.MaHang);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "TenMauSac", sanPham.MaMauSac);

            return View(sanPham);
        }

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
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "TenHang", sanPham.MaHang);
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", sanPham.MaKhuyenMai);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "TenMauSac", sanPham.MaMauSac);
            return View(sanPham);
        }

        private bool SanPhamExists(string id)
		{
			return _context.SanPhams.Any(e => e.MaSanPham == id);
		}
	}
}
