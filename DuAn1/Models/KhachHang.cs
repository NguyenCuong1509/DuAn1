using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class KhachHang
{
    public string MaKhachHang { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? Sdt { get; set; }

    public string? DiaChi { get; set; }

    public string? GioiTinh { get; set; }

    public string? TrangThai { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? MaQuanLy { get; set; }

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual QuanLy? MaQuanLyNavigation { get; set; }
}
