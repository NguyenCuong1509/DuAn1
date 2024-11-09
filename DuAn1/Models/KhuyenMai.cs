using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class KhuyenMai
{
    public string MaKhuyenMai { get; set; } = null!;

    public string? TenKhuyenMai { get; set; }

    public decimal? PhanTramGiam { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public DateTime? NgayKetThuc { get; set; }

    public string? TrangThai { get; set; }

    public string? MaQuanLy { get; set; }

    public virtual QuanLy? MaQuanLyNavigation { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
