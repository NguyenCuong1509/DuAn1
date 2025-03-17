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
    public class KhachHangsController : Controller
    {
        private readonly Duan1Context _context;

        public KhachHangsController(Duan1Context context)
        {
            _context = context;
        }

        // GET: KhachHangs
        public async Task<IActionResult> Index(string? searchString, int page = 1, int pageSize = 10)
        {
            var khachHangsQuery = _context.KhachHangs.AsQueryable();

            // Áp dụng bộ lọc tìm kiếm theo tên hoặc số điện thoại
            if (!string.IsNullOrEmpty(searchString))
            {
                khachHangsQuery = khachHangsQuery.Where(k => k.HoTen.Contains(searchString) || k.Sdt.Contains(searchString));
            }

            // Sắp xếp khách hàng theo tên
            khachHangsQuery = khachHangsQuery.OrderByDescending(k => Convert.ToInt32(k.MaKhachHang.Substring(2))); ; // Tách phần số và sắp xếp

            // Thực hiện query và lấy danh sách khách hàng với phân trang
            var totalRecords = await khachHangsQuery.CountAsync();
            var khachHangs = await khachHangsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Đặt thông tin phân trang vào ViewBag
            ViewBag.TotalRecords = totalRecords;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            // Trả về view với danh sách khách hàng đã phân trang
            return View(khachHangs);
        }
        // GET: KhachHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin khách hàng cùng với danh sách hóa đơn
            var khachHang = await _context.KhachHangs
                .Include(k => k.GioHang)
                .Include(k => k.HoaDons) // Bao gồm hóa đơn liên quan
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang); // Trả về thông tin khách hàng
        }

        // GET: KhachHangs/Create
        public IActionResult Create()
        {
            var count = _context.KhachHangs.Count();
            var maKhachHang = $"KH{count + 1}";  // Auto-generate MaKhachHang

            // Create a new KhachHang object with MaKhachHang populated
            var khachHang = new KhachHang
            {
                MaKhachHang = maKhachHang,
                TrangThai = "Đang hoạt động"
            };

            // Pass the khachHang object to the view
            return View(khachHang);
        }

        // POST: KhachHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,HoTen,Sdt,DiaChi,GioiTinh,TrangThai,Username,Password,ConfirmPassword")] KhachHang khachHang)
        {
            // Kiểm tra Username có bị trùng không
            var existingCustomer = await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.Username == khachHang.Username);

            if (existingCustomer != null)
            {
                ModelState.AddModelError("Username", "Username đã tồn tại. Vui lòng chọn Username khác.");
            }

            if (ModelState.IsValid)
            {
                // Thêm khách hàng vào cơ sở dữ liệu
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách khách hàng
            }
            return View(khachHang); // Nếu model không hợp lệ, hiển thị lại form
        }


        // POST: KhachHangs/Deactivate/5
        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                khachHang.TrangThai = "Ngừng hoạt động"; // Hoặc giá trị tương ứng
                _context.Update(khachHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: KhachHangs/Deactivate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                khachHang.TrangThai = "Đang hoạt động"; // Hoặc giá trị tương ứng
                _context.Update(khachHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
