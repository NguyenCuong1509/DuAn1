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
    public class ChiTietKhuyenMaisController : Controller
    {
        private readonly Duan1Context _context;

        public ChiTietKhuyenMaisController(Duan1Context context)
        {
            _context = context;
        }

        // GET: ChiTietKhuyenMais
        public async Task<IActionResult> Index()
        {
            await UpdateKhuyenMaiStatus();
            var duan1Context = _context.ChiTietKhuyenMais.Include(c => c.MaKhuyenMaiNavigation).Include(c => c.MaSanPhamNavigation);
            return View(await duan1Context.ToListAsync());
        }
        private async Task UpdateKhuyenMaiStatus()
        {
            var khuyenMais = await _context.KhuyenMais.Include(k => k.ChiTietKhuyenMais).ToListAsync();
            var currentDate = DateTime.Now;

            foreach (var khuyenMai in khuyenMais)
            {
                if (khuyenMai.NgayBatDau <= currentDate && khuyenMai.NgayKetThuc >= currentDate)
                {
                    khuyenMai.TrangThai = "Đang hoạt động";
                    // Cập nhật trạng thái cho ChiTietKhuyenMai
                    foreach (var chiTiet in khuyenMai.ChiTietKhuyenMais)
                    {
                        chiTiet.TrangThai = "Đang hoạt động";
                        _context.Update(chiTiet);
                    }
                }
                else if (khuyenMai.NgayKetThuc < currentDate)
                {
                    khuyenMai.TrangThai = "Ngừng hoạt động";
                    // Cập nhật trạng thái cho ChiTietKhuyenMai
                    foreach (var chiTiet in khuyenMai.ChiTietKhuyenMais)
                    {
                        chiTiet.TrangThai = "Ngừng hoạt động";
                        _context.Update(chiTiet);
                    }
                }
                else
                {
                    khuyenMai.TrangThai = "Sắp bắt đầu";
                    // Cập nhật trạng thái cho ChiTietKhuyenMai
                    foreach (var chiTiet in khuyenMai.ChiTietKhuyenMais)
                    {
                        chiTiet.TrangThai = "Sắp bắt đầu";
                        _context.Update(chiTiet);
                    }
                }

                _context.Update(khuyenMai);
            }

            await _context.SaveChangesAsync();
        }

        // GET: ChiTietKhuyenMais/Create
        public async Task<IActionResult> Create()
        {
            // Get products without active or upcoming promotions
            var products = await _context.SanPhams
                .Where(sp => sp.MaKhuyenMaiNavigation.TrangThai != "Đang hoạt động" && sp.MaKhuyenMaiNavigation.TrangThai != "Sắp bắt đầu")
                .ToListAsync();

            // Get active promotions
            var activePromotions = await _context.KhuyenMais
                .Where(km => km.TrangThai == "Đang hoạt động")
                .ToListAsync();

            ViewData["Products"] = products;
            ViewData["ActivePromotions"] = new SelectList(activePromotions, "MaKhuyenMai", "TenKhuyenMai");
            return View();
        }

        // POST: ChiTietKhuyenMais/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<string> selectedProducts, string selectedPromotion)
        {
            if (selectedProducts == null || string.IsNullOrEmpty(selectedPromotion))
            {
                ModelState.AddModelError("", "Vui lòng chọn sản phẩm và khuyến mãi.");
                return View();
            }

            // Lấy thông tin khuyến mãi theo mã khuyến mãi đã chọn
            var promotion = await _context.KhuyenMais
                .Where(km => km.MaKhuyenMai == selectedPromotion)
                .FirstOrDefaultAsync();

            // Kiểm tra nếu khuyến mãi không tồn tại
            if (promotion == null)
            {
                ModelState.AddModelError("", "Không tìm thấy khuyến mãi.");
                return View();
            }

            // Lặp qua danh sách sản phẩm đã chọn và tạo ChiTietKhuyenMai mới
            foreach (var productId in selectedProducts)
            {
                var chitietKhuyenMai = new ChiTietKhuyenMai
                {
                    MaSanPham = productId,
                    MaKhuyenMai = selectedPromotion,
                    TrangThai = "Đang hoạt động",
                    NgayBatDau = promotion.NgayBatDau, // Gán NgayBatDau từ khuyến mãi
                    NgayKetThuc = promotion.NgayKetThuc // Gán NgayKetThuc từ khuyến mãi
                };

                _context.Add(chitietKhuyenMai);
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng đến trang Index sau khi tạo thành công
            return RedirectToAction(nameof(Index));
        }


    }
}
