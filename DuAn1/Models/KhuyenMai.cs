using System;
using System.Collections.Generic;
using System.ComponentModel; // Đảm bảo bạn có thư viện này
using System.ComponentModel.DataAnnotations;  // Thêm dòng này
namespace DuAn1.Models
{
    public partial class KhuyenMai
    {
        [Key]
        [MaxLength(10)]
        [DisplayName("Mã Khuyến Mãi")]
        public string MaKhuyenMai { get; set; } = null!;

        [DisplayName("Tên Khuyến Mãi")]
        public string? TenKhuyenMai { get; set; }

        [DisplayName("Phần Trăm Giảm")]
        [Range(1, 99, ErrorMessage = "Phần trăm giảm phải lớn hơn 1 và nhỏ hơn 100.")]
        public decimal? PhanTramGiam { get; set; }

        [DisplayName("Ngày Bắt Đầu")]
        public DateTime? NgayBatDau { get; set; }

        [DisplayName("Ngày Kết Thúc")]
        public DateTime? NgayKetThuc { get; set; }

        [DisplayName("Trạng Thái")]
        public string? TrangThai { get; set; }

        [DisplayName("Mã Quản Lý")]
        [MaxLength(10)]
        public string? MaQuanLy { get; set; }
        [DisplayName("Mã Quản Lý")]

        public virtual QuanLy? QuanLy { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();

        public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; } = new List<ChiTietKhuyenMai>();
    }
}