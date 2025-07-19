using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuAn1.Migrations
{
    /// <inheritdoc />
    public partial class duan1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hang",
                columns: table => new
                {
                    MaHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TinhNang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Hang__19C0DB1DF8E2C2A8", x => x.MaHang);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKhachHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhachHan__88D2F0E5A0D6A4C0", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "MauSac",
                columns: table => new
                {
                    MaMauSac = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenMauSac = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MauSac__B9A911624FF7023E", x => x.MaMauSac);
                });

            migrationBuilder.CreateTable(
                name: "QuanLy",
                columns: table => new
                {
                    MaQuanLy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenQuanLy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QuanLy__2AB9EAF8F4C5CFE8", x => x.MaQuanLy);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLoaiSanPham = table.Column<int>(type: "int", nullable: true),
                    NgayThem = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaKhachHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GioHang__F5001DA316F5CBA4", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK__GioHang__MaKhach__66603565",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhanTramGiam = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaQuanLy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhuyenMa__6F56B3BD6B7817BE", x => x.MaKhuyenMai);
                    table.ForeignKey(
                        name: "FK__KhuyenMai__MaQua__52593CB8",
                        column: x => x.MaQuanLy,
                        principalTable: "QuanLy",
                        principalColumn: "MaQuanLy");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    MaHoaDon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    NgaySuaChua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaGioHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaKhachHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HoaDon__835ED13B1892C1EB", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK__HoaDon__MaGioHan__6A30C649",
                        column: x => x.MaGioHang,
                        principalTable: "GioHang",
                        principalColumn: "MaGioHang");
                    table.ForeignKey(
                        name: "FK__HoaDon__MaKhachH__6B24EA82",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HinhMinhHoa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    KichCo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    SoLuongTonKho = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KieuKetNoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KhoangCachKetNoi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DungLuongPin = table.Column<int>(type: "int", nullable: true),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaMauSac = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LanSuaGanNhat = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPham__FAC7442D116B707B", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK__SanPham__MaHang__5CD6CB2B",
                        column: x => x.MaHang,
                        principalTable: "Hang",
                        principalColumn: "MaHang");
                    table.ForeignKey(
                        name: "FK__SanPham__MaKhuye__5DCAEF64",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKhuyenMai");
                    table.ForeignKey(
                        name: "FK__SanPham__MaMauSa__5BE2A6F2",
                        column: x => x.MaMauSac,
                        principalTable: "MauSac",
                        principalColumn: "MaMauSac");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietK__A0FAC7FF18C39CDD", x => new { x.MaKhuyenMai, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK__ChiTietKh__MaKhu__619B8048",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKhuyenMai");
                    table.ForeignKey(
                        name: "FK__ChiTietKh__MaSan__628FA481",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "HoaDonChiTiet",
                columns: table => new
                {
                    MaHoaDon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HoaDonCh__4CF2A579A6E9C74D", x => new { x.MaHoaDon, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK__HoaDonChi__MaHoa__71D1E811",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "MaHoaDon");
                    table.ForeignKey(
                        name: "FK__HoaDonChi__MaSan__72C60C4A",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamGioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPhamG__3AAC69E1A4D8FB38", x => new { x.MaGioHang, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK__SanPhamGi__MaGio__6E01572D",
                        column: x => x.MaGioHang,
                        principalTable: "GioHang",
                        principalColumn: "MaGioHang");
                    table.ForeignKey(
                        name: "FK__SanPhamGi__MaSan__6EF57B66",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKhuyenMai_MaSanPham",
                table: "ChiTietKhuyenMai",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "UQ__GioHang__88D2F0E406D6BABF",
                table: "GioHang",
                column: "MaKhachHang",
                unique: true,
                filter: "[MaKhachHang] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaGioHang",
                table: "HoaDon",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaKhachHang",
                table: "HoaDon",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_MaSanPham",
                table: "HoaDonChiTiet",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_MaQuanLy",
                table: "KhuyenMai",
                column: "MaQuanLy");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaHang",
                table: "SanPham",
                column: "MaHang");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaKhuyenMai",
                table: "SanPham",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaMauSac",
                table: "SanPham",
                column: "MaMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamGioHang_MaSanPham",
                table: "SanPhamGioHang",
                column: "MaSanPham");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietKhuyenMai");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiet");

            migrationBuilder.DropTable(
                name: "SanPhamGioHang");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "Hang");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "MauSac");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "QuanLy");
        }
    }
}
