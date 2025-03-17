namespace DuAn1.Models
{
    public partial class SanPhamGioHang
    {
        public string MaGioHang { get; set; } = null!;

        public string MaSanPham { get; set; } = null!;

        public int? SoLuong { get; set; }

        public virtual GioHang MaGioHangNavigation { get; set; } = null!;

        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
