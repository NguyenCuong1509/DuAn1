using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAn1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DuAn1.Controllers
{
    public class GioHangController : Controller
    {
        private readonly Duan1Context _context;

        public GioHangController(Duan1Context context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _context.KhachHangs.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            var cart = await _context.GioHangs
                .Include(g => g.SanPhamGioHangs)
                    .ThenInclude(spg => spg.MaSanPhamNavigation)
                .FirstOrDefaultAsync(g => g.MaKhachHang == user.MaKhachHang);

            return View(cart); // This will pass the GioHang object to the view
        }



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
                await _context.SaveChangesAsync(); // Save the cart first to generate the MaGioHang
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
                // Only update the quantity, don't touch MaGioHang or MaSanPham
                sanPhamGioHang.SoLuong += SoLuong;
                _context.SanPhamGioHangs.Update(sanPhamGioHang);
            }

            // Update the cart with the new quantity
            cart.SoLoaiSanPham += SoLuong;
            _context.GioHangs.Update(cart);

            await _context.SaveChangesAsync(); // Save all changes
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string MaSanPham)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _context.KhachHangs.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            var cart = await _context.GioHangs.FirstOrDefaultAsync(g => g.MaKhachHang == user.MaKhachHang);
            if (cart != null)
            {
                var sanPhamGioHang = await _context.SanPhamGioHangs
                    .FirstOrDefaultAsync(sp => sp.MaGioHang == cart.MaGioHang && sp.MaSanPham == MaSanPham);

                if (sanPhamGioHang != null)
                {
                    _context.SanPhamGioHangs.Remove(sanPhamGioHang);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }



        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string MaSanPham, int SoLuong)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _context.KhachHangs.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            var cart = await _context.GioHangs.FirstOrDefaultAsync(g => g.MaKhachHang == user.MaKhachHang);
            if (cart != null)
            {
                var sanPhamGioHang = await _context.SanPhamGioHangs
                    .FirstOrDefaultAsync(sp => sp.MaGioHang == cart.MaGioHang && sp.MaSanPham == MaSanPham);

                if (sanPhamGioHang != null)
                {
                    // Update only the quantity of the product, not the foreign keys.
                    sanPhamGioHang.SoLuong = SoLuong;
                    _context.SanPhamGioHangs.Update(sanPhamGioHang);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> ThanhToan()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _context.KhachHangs.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            var cart = await _context.GioHangs
                .Include(g => g.SanPhamGioHangs)
                    .ThenInclude(spg => spg.MaSanPhamNavigation)
                        .ThenInclude(sp => sp.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(g => g.MaKhachHang == user.MaKhachHang);

            if (cart == null || !cart.SanPhamGioHangs.Any())
            {
                return RedirectToAction("Index");
            }

            // --- Kiểm tra tồn kho trước khi thanh toán ---
            foreach (var item in cart.SanPhamGioHangs)
            {
                var sp = await _context.SanPhams.FirstOrDefaultAsync(s => s.MaSanPham == item.MaSanPham);
                if (sp == null || (sp.SoLuongTonKho ?? 0) < item.SoLuong)
                {
                    TempData["ErrorMessage"] = $"Sản phẩm {sp?.TenSanPham ?? item.MaSanPham} không đủ tồn kho.";
                    return RedirectToAction("Index"); // Hoặc trang giỏ hàng
                }
            }

            // --- Tạo hóa đơn ---
            var hoaDon = new HoaDon
            {
                MaHoaDon = "HD" + (await _context.HoaDons.CountAsync() + 1),
                NgayTao = DateTime.Now,
                ThanhTien = cart.SanPhamGioHangs.Sum(item =>
                {
                    var donGia = item.MaSanPhamNavigation?.DonGia ?? 0;
                    var phanTramGiam = item.MaSanPhamNavigation?.MaKhuyenMaiNavigation?.PhanTramGiam ?? 0;
                    var giaSauGiam = donGia - (donGia * phanTramGiam / 100);
                    return item.SoLuong * giaSauGiam;
                }),
                TrangThai = "Chờ xác nhận",
                MaGioHang = cart.MaGioHang,
                MaKhachHang = cart.MaKhachHang
            };

            _context.HoaDons.Add(hoaDon);
            await _context.SaveChangesAsync();

            // --- Tạo chi tiết hóa đơn & trừ tồn kho ---
            foreach (var item in cart.SanPhamGioHangs)
            {
                var donGia = item.MaSanPhamNavigation?.DonGia ?? 0;
                var phanTramGiam = item.MaSanPhamNavigation?.MaKhuyenMaiNavigation?.PhanTramGiam ?? 0;
                var giaSauGiam = donGia - (donGia * phanTramGiam / 100);

                // Chi tiết hóa đơn
                var hoaDonChiTiet = new HoaDonChiTiet
                {
                    MaHoaDon = hoaDon.MaHoaDon,
                    MaSanPham = item.MaSanPham,
                    SoLuong = item.SoLuong,
                    DonGia = giaSauGiam
                };
                _context.HoaDonChiTiets.Add(hoaDonChiTiet);

                // Trừ tồn kho
                var sp = await _context.SanPhams.FirstOrDefaultAsync(s => s.MaSanPham == item.MaSanPham);
                if (sp != null)
                {
                    sp.SoLuongTonKho -= item.SoLuong;
                    if (sp.SoLuongTonKho < 0)
                        sp.SoLuongTonKho = 0;
                }
            }

            // --- Xóa giỏ hàng ---
            var cartItems = await _context.SanPhamGioHangs
                .Where(spg => spg.MaGioHang == cart.MaGioHang)
                .ToListAsync();
            _context.SanPhamGioHangs.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return RedirectToAction("XemHoaDon", new { maHoaDon = hoaDon.MaHoaDon });
        }


        [HttpPost]
        public async Task<IActionResult> HuyHoaDon(string MaHoaDon)
        {
            var hoaDon = await _context.HoaDons
                .Include(hd => hd.HoaDonChiTiets) // Lấy chi tiết hóa đơn
                .FirstOrDefaultAsync(hd => hd.MaHoaDon == MaHoaDon);

            if (hoaDon == null)
            {
                return RedirectToAction("Index"); // Hóa đơn không tồn tại
            }

            // --- Hoàn trả tồn kho ---
            foreach (var chiTiet in hoaDon.HoaDonChiTiets)
            {
                var sp = await _context.SanPhams.FirstOrDefaultAsync(s => s.MaSanPham == chiTiet.MaSanPham);
                if (sp != null)
                {
                    if (!sp.SoLuongTonKho.HasValue)
                        sp.SoLuongTonKho = 0;

                    sp.SoLuongTonKho += chiTiet.SoLuong; // Cộng lại số lượng
                }
            }

            // Cập nhật trạng thái hóa đơn
            hoaDon.TrangThai = "Hủy";
            _context.HoaDons.Update(hoaDon);

            await _context.SaveChangesAsync();

            return RedirectToAction("XemHoaDon", new { maHoaDon = hoaDon.MaHoaDon });
        }


        // Action để xem hóa đơn
        public async Task<IActionResult> XemHoaDon(string maHoaDon)
        {
            // Lấy hóa đơn từ cơ sở dữ liệu
            var hoaDon = await _context.HoaDons
                .Include(hd => hd.HoaDonChiTiets)
                    .ThenInclude(hdct => hdct.MaSanPhamNavigation)
                .FirstOrDefaultAsync(hd => hd.MaHoaDon == maHoaDon);

            if (hoaDon == null)
            {
                return RedirectToAction("Index"); // Nếu không tìm thấy hóa đơn, quay lại trang giỏ hàng
            }

            return View(hoaDon); // Trả về trang xem hóa đơn 
        }
    }
}
