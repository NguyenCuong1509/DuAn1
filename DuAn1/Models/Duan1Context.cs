using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DuAn1.Models;
public partial class Duan1Context : DbContext
{
    public Duan1Context()
    {
    }

    public Duan1Context(DbContextOptions<Duan1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<Hang> Hangs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<QuanLy> QuanLies { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<SanPhamGioHang> SanPhamGioHangs { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer(" Server=NMC-LP-DELL\\SQLEXPRESS;Database=DUAN1;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietKhuyenMai>(entity =>
        {
            entity.HasKey(e => new { e.MaKhuyenMai, e.MaSanPham }).HasName("PK__ChiTietK__A0FAC7FF18C39CDD");

            entity.ToTable("ChiTietKhuyenMai");

            entity.Property(e => e.MaKhuyenMai).HasMaxLength(10);
            entity.Property(e => e.MaSanPham).HasMaxLength(10);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.ChiTietKhuyenMais)
                .HasForeignKey(d => d.MaKhuyenMai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietKh__MaKhu__619B8048");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietKhuyenMais)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietKh__MaSan__628FA481");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA316F5CBA4");

            entity.ToTable("GioHang");

            entity.HasIndex(e => e.MaKhachHang, "UQ__GioHang__88D2F0E406D6BABF").IsUnique();

            entity.Property(e => e.MaGioHang).HasMaxLength(10);
            entity.Property(e => e.MaKhachHang).HasMaxLength(10);
            entity.Property(e => e.NgayThem).HasColumnType("datetime");

            entity.HasOne(d => d.MaKhachHangNavigation).WithOne(p => p.GioHang)
                .HasForeignKey<GioHang>(d => d.MaKhachHang)
                .HasConstraintName("FK__GioHang__MaKhach__66603565");
        });

        modelBuilder.Entity<Hang>(entity =>
        {
            entity.HasKey(e => e.MaHang).HasName("PK__Hang__19C0DB1DF8E2C2A8");

            entity.ToTable("Hang");

            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.TenHang).HasMaxLength(255);
            entity.Property(e => e.TinhNang).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(255);
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13B1892C1EB");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHoaDon).HasMaxLength(10);
            entity.Property(e => e.MaGioHang).HasMaxLength(10);
            entity.Property(e => e.MaKhachHang).HasMaxLength(10);
            entity.Property(e => e.NgaySuaChua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaGioHang)
                .HasConstraintName("FK__HoaDon__MaGioHan__6A30C649");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__HoaDon__MaKhachH__6B24EA82");
        });

        modelBuilder.Entity<HoaDonChiTiet>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaSanPham }).HasName("PK__HoaDonCh__4CF2A579A6E9C74D");

            entity.ToTable("HoaDonChiTiet");

            entity.Property(e => e.MaHoaDon).HasMaxLength(10);
            entity.Property(e => e.MaSanPham).HasMaxLength(10);
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.HoaDonChiTiets)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDonChi__MaHoa__71D1E811");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.HoaDonChiTiets)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDonChi__MaSan__72C60C4A");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E5A0D6A4C0");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKhachHang).HasMaxLength(10);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BD6B7817BE");

            entity.ToTable("KhuyenMai");

            entity.Property(e => e.MaKhuyenMai).HasMaxLength(10);
            entity.Property(e => e.MaQuanLy).HasMaxLength(10);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.PhanTramGiam).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TenKhuyenMai).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaQuanLyNavigation).WithMany(p => p.KhuyenMais)
                .HasForeignKey(d => d.MaQuanLy)
                .HasConstraintName("FK__KhuyenMai__MaQua__52593CB8");
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.MaMauSac).HasName("PK__MauSac__B9A911624FF7023E");

            entity.ToTable("MauSac");

            entity.Property(e => e.MaMauSac).HasMaxLength(10);
            entity.Property(e => e.TenMauSac).HasMaxLength(255);
        });

        modelBuilder.Entity<QuanLy>(entity =>
        {
            entity.HasKey(e => e.MaQuanLy).HasName("PK__QuanLy__2AB9EAF8F4C5CFE8");

            entity.ToTable("QuanLy");

            entity.Property(e => e.MaQuanLy).HasMaxLength(10);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.TenQuanLy).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__FAC7442D116B707B");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSanPham).HasMaxLength(10);
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.HinhMinhHoa).HasMaxLength(255);
            entity.Property(e => e.KhoangCachKetNoi).HasMaxLength(10);
            entity.Property(e => e.KichCo).HasMaxLength(50);
            entity.Property(e => e.KieuKetNoi).HasMaxLength(50);
            entity.Property(e => e.LanSuaGanNhat).HasColumnType("datetime");
            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.MaKhuyenMai).HasMaxLength(10);
            entity.Property(e => e.MaMauSac).HasMaxLength(10);
            entity.Property(e => e.TenSanPham).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaHang)
                .HasConstraintName("FK__SanPham__MaHang__5CD6CB2B");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__SanPham__MaKhuye__5DCAEF64");

            entity.HasOne(d => d.MaMauSacNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaMauSac)
                .HasConstraintName("FK__SanPham__MaMauSa__5BE2A6F2");
        });

        modelBuilder.Entity<SanPhamGioHang>(entity =>
        {
            entity.HasKey(e => new { e.MaGioHang, e.MaSanPham }).HasName("PK__SanPhamG__3AAC69E1A4D8FB38");

            entity.ToTable("SanPhamGioHang");

            entity.Property(e => e.MaGioHang).HasMaxLength(10);
            entity.Property(e => e.MaSanPham).HasMaxLength(10);

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.SanPhamGioHangs)
                .HasForeignKey(d => d.MaGioHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SanPhamGi__MaGio__6E01572D");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.SanPhamGioHangs)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SanPhamGi__MaSan__6EF57B66");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
