using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class Hang
    {
        [Key]
        [MaxLength(10)]
        [Display(Name = "Mã Hãng")]
        public string MaHang { get; set; } = null!;

        [MaxLength(255)]
        [Display(Name = "Tên Hãng")]
        public string? TenHang { get; set; }

        [MaxLength(255)]
        [Display(Name = "Website")]
        public string? Website { get; set; }

        [MaxLength(255)]
        [Display(Name = "Tính Năng")]
        public string? TinhNang { get; set; }

        [MaxLength(50)]
        [Display(Name = "Trạng Thái")]
        public string? TrangThai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
