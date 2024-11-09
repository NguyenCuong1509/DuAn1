using System;
using System.Collections.Generic;

namespace DuAn1.Models;

public partial class SanPham
{
    public string MaSanPham { get; set; } = null!;

    public string? TenSanPham { get; set; }

    public string? KichCo { get; set; }

    public decimal? DonGia { get; set; }

    public int? SoLuongTonKho { get; set; }

    public string? TrangThai { get; set; }

    public string? MaKhuyenMai { get; set; }

    public string? MaHang { get; set; }

    public string? MaMauSac { get; set; }

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();

    public virtual Hang? MaHangNavigation { get; set; }

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }

    public virtual MauSac? MaMauSacNavigation { get; set; }
}
