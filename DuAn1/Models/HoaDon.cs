using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class HoaDon
{
    public string MaHoaDon { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public decimal? ThanhTien { get; set; }

    public DateTime? NgaySuaChua { get; set; }

    public string? TrangThai { get; set; }

    public string? MaGioHang { get; set; }

    public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();

    public virtual GioHang? MaGioHangNavigation { get; set; }
}
