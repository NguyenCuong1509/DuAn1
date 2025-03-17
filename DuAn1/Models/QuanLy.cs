using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DuAn1.Models;

public partial class QuanLy
{
    [DisplayName("Mã Quản Lý")]
    public string MaQuanLy { get; set; } = null!;

    [DisplayName("Tên Quản Lý")]
    public string? TenQuanLy { get; set; }

    [DisplayName("Ngày Sinh")]
    public DateOnly? NgaySinh { get; set; }

    [DisplayName("Địa Chỉ")]
    public string? DiaChi { get; set; }

    [DisplayName("Giới Tính")]
    public string? GioiTinh { get; set; }

    [DisplayName("Trạng Thái")]
    public string? TrangThai { get; set; }

    [DisplayName("Tên Đăng Nhập")]
    public string? Username { get; set; }

    [DisplayName("Mật Khẩu")]
    public string? Password { get; set; }

    public virtual ICollection<KhuyenMai> KhuyenMais { get; set; } = new List<KhuyenMai>();
}
