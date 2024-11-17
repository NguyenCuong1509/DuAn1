using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuAn1.Models;

public partial class SanPham
{
    [Display(Name = "Mã Sản Phẩm")]
    public string MaSanPham { get; set; } = null!;

    [Display(Name = "Tên Sản Phẩm")]
    public string? TenSanPham { get; set; }

    [Display(Name = "Hình Minh Họa")]
    public string? HinhMinhHoa { get; set; }

    [Display(Name = "Kích Cỡ")]
    public string? KichCo { get; set; }

    [Display(Name = "Đơn Giá")]
    public decimal? DonGia { get; set; }

    [Display(Name = "Tồn Kho")]
    public int? SoLuongTonKho { get; set; }

    [Display(Name = "Trạng Thái")]
    public string? TrangThai { get; set; }

    [Display(Name = "Kiểu Kết Nối")]
    public string? KieuKetNoi { get; set; }

    [Display(Name = "Khoảng Cách")]
    public string? KhoangCachKetNoi { get; set; }

    [Display(Name = "Pin")]
    public int? DungLuongPin { get; set; }

    [Display(Name = "Mã Khuyến Mãi")]
    public string? MaKhuyenMai { get; set; }

    [Display(Name = "Mã Hàng")]
    public string? MaHang { get; set; }

    [Display(Name = "Mã Màu Sắc")]
    public string? MaMauSac { get; set; }

    [Display(Name = "Lần Sửa Gần Nhất")]
    public DateTime? LanSuaGanNhat { get; set; }

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();

    [Display(Name = "Hãng")]
    public virtual Hang? MaHangNavigation { get; set; }

    [Display(Name = "Khuyến Mãi")]
    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }

    [Display(Name = "Màu Sắc")]
    public virtual MauSac? MaMauSacNavigation { get; set; }
}
