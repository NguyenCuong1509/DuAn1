using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAn1.Models;

namespace DuAn1.Controllers
{
    public class KhachHangsController : Controller
    {
        private readonly Duan1Context _context;
        private const string ActiveStatus = "Đang hoạt động";
        private const string InactiveStatus = "Ngừng hoạt động";

        public KhachHangsController(Duan1Context context)
        {
            _context = context;
        }

        // GET: KhachHangs
        public async Task<IActionResult> Index(string searchString, int page = 1, int pageSize = 10)
        {
            // Ensure page and pageSize are valid
            page = Math.Max(1, page);
            pageSize = Math.Max(1, Math.Min(pageSize, 50)); // Cap pageSize to avoid abuse

            // Build query with AsNoTracking for read-only
            var khachHangsQuery = _context.KhachHangs
                .AsNoTracking()
                .Select(k => new KhachHang
                {
                    MaKhachHang = k.MaKhachHang,
                    HoTen = k.HoTen,
                    Sdt = k.Sdt,
                    DiaChi = k.DiaChi,
                    GioiTinh = k.GioiTinh,
                    TrangThai = k.TrangThai
                });

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                khachHangsQuery = khachHangsQuery.Where(k => k.HoTen.ToLower().Contains(searchString) || k.Sdt.Contains(searchString));
            }

            // Sort by MaKhachHang (assuming it's an integer-like string, e.g., "KH123")
            khachHangsQuery = khachHangsQuery.OrderByDescending(k => k.MaKhachHang);

            // Get total count for pagination
            int totalRecords = await khachHangsQuery.CountAsync();

            // Fetch paginated data
            var khachHangs = await khachHangsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Pass pagination data to view
            ViewBag.TotalRecords = totalRecords;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(khachHangs);
        }

        // GET: KhachHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Fetch only required fields with AsNoTracking
            var khachHang = await _context.KhachHangs
                .AsNoTracking()
                .Select(k => new KhachHang
                {
                    MaKhachHang = k.MaKhachHang,
                    HoTen = k.HoTen,
                    Sdt = k.Sdt,
                    DiaChi = k.DiaChi,
                    GioiTinh = k.GioiTinh,
                    TrangThai = k.TrangThai,
                    Username = k.Username,
                    GioHang = k.GioHang,
                    HoaDons = k.HoaDons.Select(h => new HoaDon
                    {
                        MaHoaDon = h.MaHoaDon,
                        NgayTao = h.NgayTao,
                        ThanhTien = h.ThanhTien,
                        TrangThai = h.TrangThai
                    }).ToList()
                })
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public IActionResult Create()
        {
            // Generate MaKhachHang in the database using a stored procedure or trigger (recommended)
            // For simplicity, we'll keep the logic here, but consider moving to DB
            var count = _context.KhachHangs.Count() + 1;
            var khachHang = new KhachHang
            {
                MaKhachHang = $"KH{count}",
                TrangThai = ActiveStatus
            };

            return View(khachHang);
        }

        // POST: KhachHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,HoTen,Sdt,DiaChi,GioiTinh,TrangThai,Username,Password")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Hash password before saving (basic example, use proper hashing like BCrypt in production)
                khachHang.Password = HashPassword(khachHang.Password);

                await _context.Database.BeginTransactionAsync();
                try
                {
                    // Add customer
                    _context.Add(khachHang);
                    await _context.SaveChangesAsync();

                    // Create cart
                    var gioHang = new GioHang
                    {
                        MaGioHang = $"GH{_context.GioHangs.Count() + 1}",
                        MaKhachHang = khachHang.MaKhachHang,
                        NgayThem = DateTime.UtcNow,
                        SoLoaiSanPham = 0
                    };
                    _context.GioHangs.Add(gioHang);
                    await _context.SaveChangesAsync();

                    await _context.Database.CommitTransactionAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    await _context.Database.RollbackTransactionAsync();
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo khách hàng.");
                }
            }

            return View(khachHang);
        }

        // POST: KhachHangs/Deactivate/5
        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                khachHang.TrangThai = InactiveStatus;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: KhachHangs/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                khachHang.TrangThai = ActiveStatus;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Placeholder for password hashing (implement with proper library in production)
        private string HashPassword(string password)
        {
            // Use BCrypt or ASP.NET Identity's PasswordHasher in production
            return password; // Temporary placeholder
        }
    }
}