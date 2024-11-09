using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class MauSac
{
    public string MaMauSac { get; set; } = null!;

    public string? TenMauSac { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
