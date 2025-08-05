using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class GioHang
    {
        [Key]
        [MaxLength(10)]
        public string MaGioHang { get; set; } = null!;

        public int? SoLoaiSanPham { get; set; }

        public DateTime? NgayThem { get; set; }

        [MaxLength(10)]
        public string? MaKhachHang { get; set; }

        [ForeignKey(nameof(MaKhachHang))]
        public virtual KhachHang? MaKhachHangNavigation { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

        public virtual ICollection<SanPhamGioHang> SanPhamGioHangs { get; set; } = new List<SanPhamGioHang>();
    }
}
