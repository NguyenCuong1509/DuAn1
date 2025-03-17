using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuAn1.Models;

public partial class KhachHang
{
    public string MaKhachHang { get; set; } = null!;
    [Display(Name = "Họ và tên")]
    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    public string? HoTen { get; set; }
    [Display(Name = "Số điện thoại")]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và không có ký tự đặc biệt.")]
    public string? Sdt { get; set; }

    [Display(Name = "Địa chỉ")]
    [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
    public string? DiaChi { get; set; }

    [Required(ErrorMessage = "Giới tính là bắt buộc.")]
    [Display(Name = "Giới tính")]

    public string? GioiTinh { get; set; }


    [Display(Name = "Trạng thái")]
    public string? TrangThai { get; set; }

    [Display(Name = "Tên đăng nhập")]

    [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
    public string? Username { get; set; }

    [Display(Name = "Mật khẩu")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    public string? Password { get; set; }

    public virtual GioHang? GioHang { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

}
