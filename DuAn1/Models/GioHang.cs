using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class GioHang
{
    public string MaGioHang { get; set; } = null!;

    public int? SoLuong { get; set; }

    public DateTime? NgayThem { get; set; }

    public string? MaSanPham { get; set; }

    public string? MaKhachHang { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
