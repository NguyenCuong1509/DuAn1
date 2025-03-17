namespace DuAn1.Models
{
    public partial class ChiTietKhuyenMai
    {
        public string MaKhuyenMai { get; set; } = null!;

        public string MaSanPham { get; set; } = null!;

        public DateTime? NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        public string? TrangThai { get; set; }

        public virtual KhuyenMai MaKhuyenMaiNavigation { get; set; } = null!;

        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }

}
