using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuAn1.Models;

public partial class HoaDon
{
    [Display(Name = "Mã Hóa Đơn")]
    public string MaHoaDon { get; set; } = null!;

    [Display(Name = "Ngày Tạo")]
    public DateTime? NgayTao { get; set; }

    [Display(Name = "Thành Tiền")]
    public decimal? ThanhTien { get; set; }

    [Display(Name = "Ngày Sửa Chữa")]
    public DateTime? NgaySuaChua { get; set; }

    [Display(Name = "Trạng Thái")]
    public string? TrangThai { get; set; }

    [Display(Name = "Mã Giỏ Hàng")]
    public string? MaGioHang { get; set; }

    [Display(Name = "Mã Khách Hàng")]
    public string? MaKhachHang { get; set; }

    public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();

    public virtual GioHang? MaGioHangNavigation { get; set; }
    [Display(Name = "Mã Khách Hàng")]
    public virtual KhachHang? MaKhachHangNavigation { get; set; }
}
