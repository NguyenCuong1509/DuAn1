using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class ChiTietKhuyenMai
    {
        [Key, Column(Order = 0)]
        [MaxLength(10)]
        public string MaKhuyenMai { get; set; } = null!;

        [Key, Column(Order = 1)]
        [MaxLength(10)]
        public string MaSanPham { get; set; } = null!;

        public DateTime? NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        [MaxLength(50)]
        public string? TrangThai { get; set; }

        [ForeignKey(nameof(MaKhuyenMai))]
        public virtual KhuyenMai MaKhuyenMaiNavigation { get; set; } = null!;

        [ForeignKey(nameof(MaSanPham))]
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
