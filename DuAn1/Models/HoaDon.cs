using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class HoaDon
    {
        [Key]
        [MaxLength(10)]
        [Display(Name = "Mã Hóa Đơn")]
        public string MaHoaDon { get; set; } = null!;

        [Display(Name = "Ngày Tạo")]
        public DateTime? NgayTao { get; set; }

        [Display(Name = "Thành Tiền")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ThanhTien { get; set; }

        [Display(Name = "Ngày Sửa Chữa")]
        public DateTime? NgaySuaChua { get; set; }

        [Display(Name = "Trạng Thái")]
        [MaxLength(50)]
        public string? TrangThai { get; set; }

        [Display(Name = "Mã Giỏ Hàng")]
        [MaxLength(10)]
        public string? MaGioHang { get; set; }

        [Display(Name = "Mã Khách Hàng")]
        [MaxLength(10)]
        public string? MaKhachHang { get; set; }

        [ForeignKey(nameof(MaGioHang))]
        public virtual GioHang? MaGioHangNavigation { get; set; }

        [ForeignKey(nameof(MaKhachHang))]
        public virtual KhachHang? MaKhachHangNavigation { get; set; }

        public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
    }
}
