using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DuAn1.Models;

public partial class QuanLy
{
    [Key]
    [MaxLength(10)]
    [Required]
    [DisplayName("Mã Quản Lý")]
    public string MaQuanLy { get; set; } = null!;

    [Required, MaxLength(100)]
    [DisplayName("Tên Quản Lý")]
    public string TenQuanLy { get; set; } = null!;

    [Required]
    [DisplayName("Ngày Sinh")]
    public DateOnly NgaySinh { get; set; }

    [Required, MaxLength(255)]
    [DisplayName("Địa Chỉ")]
    public string DiaChi { get; set; } = null!;

    [Required, MaxLength(10)]
    [DisplayName("Giới Tính")]
    public string GioiTinh { get; set; } = null!;

    [Required]
    [DisplayName("Trạng Thái")]
    public string TrangThai { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Tên đăng nhập phải từ 4 đến 50 ký tự")]
    [DisplayName("Tên Đăng Nhập")]
    public string Username { get; set; } = null!;
    
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 50 ký tự")]
    [DisplayName("Mật Khẩu")]
    public string? Password { get; set; } = null!;

    public virtual ICollection<KhuyenMai> KhuyenMais { get; set; } = new List<KhuyenMai>();
}
