using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class Hang
    {
        [Key]
        [MaxLength(10)]
        public string MaHang { get; set; } = null!;

        [MaxLength(255)]
        public string? TenHang { get; set; }

        [MaxLength(255)]
        public string? Website { get; set; }

        [MaxLength(255)]
        public string? TinhNang { get; set; }

        [MaxLength(50)]
        public string? TrangThai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
