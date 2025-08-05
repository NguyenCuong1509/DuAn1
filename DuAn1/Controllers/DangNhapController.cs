using Microsoft.AspNetCore.Mvc;
using DuAn1.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DuAn1.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly Duan1Context _context;

        // Inject DbContext vào controller
        public DangNhapController(Duan1Context context)
        {
            _context = context;
        }

        // Hiển thị form đăng nhập
        // GET: Đăng nhập
        public IActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangNhap(string username, string password)
        {
            // Kiểm tra thông tin người dùng trong database (khách hàng)
            var user = _context.KhachHangs.FirstOrDefault(u => u.Username == username && u.Password == password);
            var ql = _context.QuanLies.FirstOrDefault(q => q.Username == username && q.Password == password);

            if (user == null && ql == null)
            {
                ViewData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }

            // Kiểm tra nếu là khách hàng
            if (user != null)
            {
                // Kiểm tra trạng thái khách hàng
                if (user.TrangThai == "Ngừng hoạt động")
                {
                    ViewData["ErrorMessage"] = "Tài khoản của bạn đã bị ngừng hoạt động!";
                    return View();
                }

                // Lưu thông tin khách hàng vào session
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", "KhachHang"); // Lưu role là Khách hàng
                HttpContext.Session.SetString("MaKhachHang", user.MaKhachHang); // Lưu mã khách hàng vào session

                // Kiểm tra nếu người dùng chưa có giỏ hàng
                var cart = _context.GioHangs.FirstOrDefault(g => g.MaKhachHang == user.MaKhachHang);
                if (cart == null)
                {
                    // Nếu không có giỏ hàng, tạo mới
                    cart = new GioHang
                    {
                        MaKhachHang = user.MaKhachHang,
                        NgayThem = DateTime.Now
                    };
                    _context.GioHangs.Add(cart);
                    _context.SaveChanges();
                }

                // Chuyển hướng đến trang chủ dành cho khách hàng
                return RedirectToAction("Index", "TrangBanSanPhams");
            }
            // Nếu là quản lý
            else if (ql != null)
            {
                // Kiểm tra trạng thái quản lý (nếu có yêu cầu)
                if (ql.TrangThai == "Ngừng hoạt động")
                {
                    ViewData["ErrorMessage"] = "Tài khoản quản lý của bạn đã bị ngừng hoạt động!";
                    return View();
                }

                // Lưu thông tin quản lý vào session
                HttpContext.Session.SetString("UsernameQl", ql.Username);
                HttpContext.Session.SetString("Role", "QuanLy"); // Lưu role là Quản lý
                HttpContext.Session.SetString("MaQuanLy", ql.MaQuanLy); // Lưu mã quản lý vào session
                                                                        // Chuyển hướng đến trang quản lý
                return RedirectToAction("Details", "QuanLy", new { id = ql.MaQuanLy });
            }

            // Nếu không có tài khoản hợp lệ
            ViewData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }


        // Đăng xuất
        public IActionResult Logout()
        {
            // Xóa thông tin người dùng và quyền hạn khỏi session khi đăng xuất
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Role");

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("DangNhap", "DangNhap");
        }

    }
}

