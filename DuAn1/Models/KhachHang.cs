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
        [MaxLength(255)]
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
        [MaxLength(50)]
        public string? Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MaxLength(100)]
        public string? Password { get; set; }

        // Điều hướng tới các thực thể liên quan
        public virtual GioHang? GioHang { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}
