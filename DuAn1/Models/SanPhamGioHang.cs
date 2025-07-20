using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class SanPhamGioHang
    {
        [Key, Column(Order = 0)]
        [MaxLength(10)]
        public string MaGioHang { get; set; } = null!;

        [Key, Column(Order = 1)]
        [MaxLength(10)]
        public string MaSanPham { get; set; } = null!;

        public int? SoLuong { get; set; }

        [ForeignKey(nameof(MaGioHang))]
        public virtual GioHang MaGioHangNavigation { get; set; } = null!;

        [ForeignKey(nameof(MaSanPham))]
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}