using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class KhachHang
    {
        [Key]
        [MaxLength(10)]
        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; } = null!;

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [MaxLength(100)]
        public string? HoTen { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và không có ký tự đặc biệt.")]
        [MaxLength(10)]
        public string? Sdt { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
        [RegularExpression(@"^.+,\s*.+,\s*.+,\s*.+$",
               ErrorMessage = "Địa chỉ phải theo định dạng Thôn/Xã – Huyện – TP, ví dụ: Thôn A, Xã B, Huyện C, TP D.")]
        public string? DiaChi { get; set; }
        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Giới tính là bắt buộc.")]
        [MaxLength(10)]
        public string? GioiTinh { get; set; }

        [Display(Name = "Trạng thái")]
        [MaxLength(20)]
        public string? TrangThai { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        [MinLength(4, ErrorMessage = "Tên đăng nhập phải ít nhất 8 ký tự.")]
        [MaxLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự.")]
        public string? Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải ít nhất 8 ký tự.")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự.")]
        public string? Password { get; set; }

        // Điều hướng tới các thực thể liên quan
        public virtual GioHang? GioHang { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}
