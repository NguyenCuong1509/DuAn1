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

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<Hang> Hangs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<QuanLy> QuanLies { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=NMC-LP-DELL\\SQLEXPRESS;Database=DUAN1;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA3D7C787A4");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGioHang).HasMaxLength(10);
            entity.Property(e => e.MaKhachHang).HasMaxLength(10);
            entity.Property(e => e.MaSanPham).HasMaxLength(10);
            entity.Property(e => e.NgayThem).HasColumnType("datetime");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__GioHang__MaKhach__619B8048");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK__GioHang__MaSanPh__60A75C0F");
        });

        modelBuilder.Entity<Hang>(entity =>
        {
            entity.HasKey(e => e.MaHang).HasName("PK__Hang__19C0DB1D62C3EEEB");

            entity.ToTable("Hang");

            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.TenHang).HasMaxLength(255);
            entity.Property(e => e.TinhNang).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(255);
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13B154D305E");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHoaDon).HasMaxLength(10);
            entity.Property(e => e.MaGioHang).HasMaxLength(10);
            entity.Property(e => e.NgaySuaChua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaGioHang)
                .HasConstraintName("FK__HoaDon__MaGioHan__656C112C");
        });

        modelBuilder.Entity<HoaDonChiTiet>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaSanPham }).HasName("PK__HoaDonCh__4CF2A579A4D70022");

            entity.ToTable("HoaDonChiTiet");

            entity.Property(e => e.MaHoaDon).HasMaxLength(10);
            entity.Property(e => e.MaSanPham).HasMaxLength(10);
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.HoaDonChiTiets)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDonChi__MaHoa__68487DD7");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.HoaDonChiTiets)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDonChi__MaSan__693CA210");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E52A048581");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKhachHang).HasMaxLength(10);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(255);
            entity.Property(e => e.MaQuanLy).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.MaQuanLyNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.MaQuanLy)
                .HasConstraintName("FK__KhachHang__MaQua__4F7CD00D");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BDE4614039");

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
                .HasConstraintName("FK__KhuyenMai__MaQua__534D60F1");
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.MaMauSac).HasName("PK__MauSac__B9A91162717D700A");

            entity.ToTable("MauSac");

            entity.Property(e => e.MaMauSac).HasMaxLength(10);
            entity.Property(e => e.TenMauSac).HasMaxLength(255);
        });

        modelBuilder.Entity<QuanLy>(entity =>
        {
            entity.HasKey(e => e.MaQuanLy).HasName("PK__QuanLy__2AB9EAF82C86E90B");

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
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__FAC7442DFDE20C43");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSanPham).HasMaxLength(10);
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.KichCo).HasMaxLength(50);
            entity.Property(e => e.KhoangCachKetNoi).HasMaxLength(10);
            entity.Property(e => e.MaHang).HasMaxLength(10);
            entity.Property(e => e.MaKhuyenMai).HasMaxLength(10);
            entity.Property(e => e.MaMauSac).HasMaxLength(10);
            entity.Property(e => e.TenSanPham).HasMaxLength(255);
            entity.Property(e => e.HinhMinhHoa).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.KieuKetNoi).HasMaxLength(50);
            entity.Property(e => e.LanSuaGanNhat).HasColumnType("datetime");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
