using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class HoaDonChiTiet
{
    public string MaHoaDon { get; set; } = null!;

    public string MaSanPham { get; set; } = null!;

    public int? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
