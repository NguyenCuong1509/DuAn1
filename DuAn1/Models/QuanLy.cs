using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class QuanLy
{
    public string MaQuanLy { get; set; } = null!;

    public string? TenQuanLy { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? GioiTinh { get; set; }

    public string? TrangThai { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<KhuyenMai> KhuyenMais { get; set; } = new List<KhuyenMai>();
}
