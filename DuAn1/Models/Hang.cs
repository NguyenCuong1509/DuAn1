using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class Hang
{
    public string MaHang { get; set; } = null!;

    public string? TenHang { get; set; }

    public string? Website { get; set; }

    public string? TinhNang { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
