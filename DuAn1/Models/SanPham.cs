using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAn1.Models
{
    public class SanPham
    {
        [Key]
        [MaxLength(10)]
        [Display(Name = "Mã Sản Phẩm")]
        public string MaSanPham { get; set; } = null!;

        [MaxLength(255)]
        [Display(Name = "Tên Sản Phẩm")]
        public string? TenSanPham { get; set; }

        [MaxLength(255)]
        [Display(Name = "Hình Minh Họa")]
        public string? HinhMinhHoa { get; set; }

        [MaxLength(100)]
        [Display(Name = "Kích Cỡ")]
        public string? KichCo { get; set; }

        [Display(Name = "Đơn Giá")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không được âm.")]
        public decimal? DonGia { get; set; }

        [Display(Name = "Tồn Kho")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm.")]
        public int? SoLuongTonKho { get; set; }

        [MaxLength(50)]
        [Display(Name = "Trạng Thái")]
        public string? TrangThai { get; set; }

        [MaxLength(50)]
        [Display(Name = "Kiểu Kết Nối")]
        public string? KieuKetNoi { get; set; }

        [MaxLength(50)]
        [Display(Name = "Khoảng Cách")]
        public string? KhoangCachKetNoi { get; set; }

        [Display(Name = "Pin")]
        [Range(0, int.MaxValue, ErrorMessage = "Dung lượng pin không được âm.")]
        public int? DungLuongPin { get; set; }

        [MaxLength(10)]
        [Display(Name = "Mã Khuyến Mãi")]
        public string? MaKhuyenMai { get; set; }

        [MaxLength(10)]
        [Display(Name = "Mã Hãng")]
        public string? MaHang { get; set; }

        [MaxLength(10)]
        [Display(Name = "Mã Màu Sắc")]
        public string? MaMauSac { get; set; }

        [Display(Name = "Lần Sửa Gần Nhất")]
        public DateTime? LanSuaGanNhat { get; set; }

        [ForeignKey(nameof(MaHang))]
        [Display(Name = "Hãng")]
        public virtual Hang? MaHangNavigation { get; set; }

        [ForeignKey(nameof(MaKhuyenMai))]
        [Display(Name = "Khuyến Mãi")]
        public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }

        [ForeignKey(nameof(MaMauSac))]
        [Display(Name = "Màu Sắc")]
        public virtual MauSac? MaMauSacNavigation { get; set; }

        public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; } = new List<ChiTietKhuyenMai>();
        public virtual ICollection<SanPhamGioHang> SanPhamGioHangs { get; set; } = new List<SanPhamGioHang>();
        public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
    }
}
