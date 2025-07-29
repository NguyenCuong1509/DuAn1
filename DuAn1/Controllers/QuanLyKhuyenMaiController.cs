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
    public class QuanLyKhuyenMaiController : Controller
    {
        private readonly Duan1Context _context;

        public QuanLyKhuyenMaiController(Duan1Context context)
        {
            _context = context;
        }

        // GET: QuanLyKhuyenMai
        // GET: QuanLyKhuyenMai
        public async Task<IActionResult> Index()
        {
            await UpdateKhuyenMaiStatus();

            // Sắp xếp các khuyến mãi theo MaKhuyenMai từ cao xuống thấp
            var duan1Context = _context.KhuyenMais
                                        .Include(k => k.QuanLy)
                                        .OrderByDescending(k => Convert.ToInt32(k.MaKhuyenMai.Substring(2)));  // Loại bỏ phần "KM" và chuyển sang số

            return View(await duan1Context.ToListAsync());
        }


        // GET: QuanLyKhuyenMai/Details/5
        // GET: QuanLyKhuyenMai/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái khuyến mãi trước khi hiển thị trang chi tiết
            await UpdateKhuyenMaiStatus();

            var khuyenMai = await _context.KhuyenMais
                .Include(k => k.QuanLy)
                .Include(k => k.ChiTietKhuyenMais) // Bao gồm ChiTietKhuyenMais
                .ThenInclude(ct => ct.MaSanPhamNavigation) // Bao gồm thông tin sản phẩm
                .FirstOrDefaultAsync(m => m.MaKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }


        // GET: QuanLyKhuyenMai/Create
        public async Task<IActionResult> Create()
        {
            // Lấy MaQuanLy từ session
            var maQuanLy = HttpContext.Session.GetString("MaQuanLy");

            // Kiểm tra nếu không tìm thấy MaQuanLy trong session, chuyển hướng đến trang đăng nhập
            if (maQuanLy == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            // Tạo mã khuyến mãi tự động bằng cách lấy số lượng khuyến mãi hiện tại và cộng thêm 1
            string newKmCode;

            // Lấy tổng số khuyến mãi hiện tại
            int totalKhuyenMais = await _context.KhuyenMais.CountAsync();

            // Tạo mã khuyến mãi mới, mã sẽ có dạng KM{Số lượng khuyến mãi + 1}
            newKmCode = "KM" + (totalKhuyenMais + 1).ToString();

            // Tạo đối tượng KhuyenMai mới và gán MaQuanLy từ session
            var khuyenMai = new KhuyenMai
            {
                MaKhuyenMai = newKmCode,
                MaQuanLy = maQuanLy
            };


            // Gán trạng thái dựa theo ngày nhập
            if (khuyenMai.NgayBatDau > DateTime.Now)
            {
                khuyenMai.TrangThai = "Chưa bắt đầu";
            }
            else if (khuyenMai.NgayKetThuc < DateTime.Now)
            {
                khuyenMai.TrangThai = "Đã kết thúc";
            }
            else
            {
                khuyenMai.TrangThai = "Đang hoạt động";
            }


            // Tạo SelectList cho dropdown, mặc định chọn MaQuanLy từ session
            ViewData["MaQuanLy"] = new SelectList(_context.QuanLies, "MaQuanLy", "MaQuanLy", maQuanLy);

            // Trả về view với đối tượng KhuyenMai mới
            return View(khuyenMai);
        }

        // POST: QuanLyKhuyenMai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhuyenMai,TenKhuyenMai,PhanTramGiam,NgayBatDau,NgayKetThuc,TrangThai")] KhuyenMai khuyenMai)
        {
            // Lấy MaQuanLy từ session
            var maQuanLy = HttpContext.Session.GetString("MaQuanLy");

            if (maQuanLy == null)
            {
                // Nếu không tìm thấy MaQuanLy trong session, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "DangNhap");
            }

            // Gán MaQuanLy cho khuyến mãi
            khuyenMai.MaQuanLy = maQuanLy;

            // Kiểm tra xem dữ liệu nhập vào có hợp lệ không
            if (ModelState.IsValid)
            {
                _context.Add(khuyenMai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, giữ lại dữ liệu và các thông tin cần thiết cho dropdown
            ViewData["MaQuanLy"] = new SelectList(_context.QuanLies, "MaQuanLy", "MaQuanLy", khuyenMai.MaQuanLy);
            return View(khuyenMai);
        }

        // GET: QuanLyKhuyenMai/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            ViewData["MaQuanLy"] = new SelectList(_context.QuanLies, "MaQuanLy", "MaQuanLy", khuyenMai.MaQuanLy);
            return View(khuyenMai);
        }

        // POST: QuanLyKhuyenMai/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKhuyenMai,TenKhuyenMai,PhanTramGiam,NgayBatDau,NgayKetThuc,TrangThai,MaQuanLy")] KhuyenMai khuyenMai)
        {
            if (id != khuyenMai.MaKhuyenMai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khuyenMai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhuyenMaiExists(khuyenMai.MaKhuyenMai))
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
            ViewData["MaQuanLy"] = new SelectList(_context.QuanLies, "MaQuanLy", "MaQuanLy", khuyenMai.MaQuanLy);
            return View(khuyenMai);
        }

        private bool KhuyenMaiExists(string id)
        {
            return _context.KhuyenMais.Any(e => e.MaKhuyenMai == id);
        }

        private async Task UpdateKhuyenMaiStatus()
        {
            var khuyenMais = await _context.KhuyenMais.ToListAsync();
            var currentDate = DateTime.Now;


            foreach (var khuyenMai in khuyenMais)
            {
                if (khuyenMai.NgayBatDau <= currentDate && khuyenMai.NgayKetThuc >= currentDate)
                {
                    khuyenMai.TrangThai = "Đang hoạt động";
                }
                else if (khuyenMai.NgayKetThuc < currentDate)
                {
                    khuyenMai.TrangThai = "Ngừng hoạt động";
                }
                else
                {
                    khuyenMai.TrangThai = "Sắp bắt đầu";
                }

                _context.Update(khuyenMai);
            }

            await _context.SaveChangesAsync();
        }
    }
}
