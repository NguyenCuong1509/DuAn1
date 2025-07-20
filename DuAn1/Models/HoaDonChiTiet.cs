using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class HoaDonChiTiet
    {
        [Key, Column(Order = 0)]
        [MaxLength(10)]
        [Display(Name = "Mã Hóa Đơn")]
        public string MaHoaDon { get; set; } = null!;

        [Key, Column(Order = 1)]
        [MaxLength(10)]
        [Display(Name = "Mã Sản Phẩm")]
        public string MaSanPham { get; set; } = null!;

        [Display(Name = "Số Lượng")]
        public int? SoLuong { get; set; }

        [Display(Name = "Đơn Giá")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? DonGia { get; set; }

        [ForeignKey(nameof(MaHoaDon))]
        public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;

        [ForeignKey(nameof(MaSanPham))]
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
