using Microsoft.EntityFrameworkCore;

namespace DuAn1.Models
{
    public class Duan1Context : DbContext
    {
        public Duan1Context(DbContextOptions<Duan1Context> options) : base(options) { }

        public DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<Hang> Hangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<QuanLy> QuanLies { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<SanPhamGioHang> SanPhamGioHangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietKhuyenMai>().HasKey(c => new { c.MaKhuyenMai, c.MaSanPham });

            modelBuilder.Entity<GioHang>()
                .HasOne(g => g.MaKhachHangNavigation)
                .WithOne(k => k.GioHang)
                .HasForeignKey<GioHang>(g => g.MaKhachHang);

            modelBuilder.Entity<HoaDonChiTiet>().HasKey(hd => new { hd.MaHoaDon, hd.MaSanPham });

            modelBuilder.Entity<SanPhamGioHang>().HasKey(sp => new { sp.MaGioHang, sp.MaSanPham });
        }
    }
}
