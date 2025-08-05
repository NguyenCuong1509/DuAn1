using DuAn1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class MauSac
    {
        [Key]
        [MaxLength(10)]
        public string MaMauSac { get; set; } = null!;

        [MaxLength(100)]
        public string? TenMauSac { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }

}