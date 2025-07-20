using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuAn1.Migrations
{
    /// <inheritdoc />
    public partial class cf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hangs",
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
                    table.PrimaryKey("PK_Hangs", x => x.MaHang);
                });

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    MaKhachHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "MauSacs",
                columns: table => new
                {
                    MaMauSac = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenMauSac = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSacs", x => x.MaMauSac);
                });

            migrationBuilder.CreateTable(
                name: "QuanLies",
                columns: table => new
                {
                    MaQuanLy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenQuanLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLies", x => x.MaQuanLy);
                });

            migrationBuilder.CreateTable(
                name: "GioHangs",
                columns: table => new
                {
                    MaGioHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLoaiSanPham = table.Column<int>(type: "int", nullable: true),
                    NgayThem = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaKhachHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangs", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_GioHangs_KhachHangs_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMais",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhanTramGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaQuanLy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QuanLyMaQuanLy = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMais", x => x.MaKhuyenMai);
                    table.ForeignKey(
                        name: "FK_KhuyenMais_QuanLies_QuanLyMaQuanLy",
                        column: x => x.QuanLyMaQuanLy,
                        principalTable: "QuanLies",
                        principalColumn: "MaQuanLy");
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    MaHoaDon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    NgaySuaChua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaGioHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaKhachHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_GioHangs_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "GioHangs",
                        principalColumn: "MaGioHang");
                    table.ForeignKey(
                        name: "FK_HoaDons_KhachHangs_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HinhMinhHoa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    KichCo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    SoLuongTonKho = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KieuKetNoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KhoangCachKetNoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DungLuongPin = table.Column<int>(type: "int", nullable: true),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaMauSac = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LanSuaGanNhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_SanPhams_Hangs_MaHang",
                        column: x => x.MaHang,
                        principalTable: "Hangs",
                        principalColumn: "MaHang");
                    table.ForeignKey(
                        name: "FK_SanPhams_KhuyenMais_MaKhuyenMai",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMais",
                        principalColumn: "MaKhuyenMai");
                    table.ForeignKey(
                        name: "FK_SanPhams_MauSacs_MaMauSac",
                        column: x => x.MaMauSac,
                        principalTable: "MauSacs",
                        principalColumn: "MaMauSac");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKhuyenMais",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKhuyenMais", x => new { x.MaKhuyenMai, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietKhuyenMais_KhuyenMais_MaKhuyenMai",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMais",
                        principalColumn: "MaKhuyenMai",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietKhuyenMais_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonChiTiets",
                columns: table => new
                {
                    MaHoaDon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonChiTiets", x => new { x.MaHoaDon, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiets_HoaDons_MaHoaDon",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiets_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPhamGioHangs",
                columns: table => new
                {
                    MaGioHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamGioHangs", x => new { x.MaGioHang, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK_SanPhamGioHangs_GioHangs_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "GioHangs",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhamGioHangs_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKhuyenMais_MaSanPham",
                table: "ChiTietKhuyenMais",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangs_MaKhachHang",
                table: "GioHangs",
                column: "MaKhachHang",
                unique: true,
                filter: "[MaKhachHang] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiets_MaSanPham",
                table: "HoaDonChiTiets",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaGioHang",
                table: "HoaDons",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaKhachHang",
                table: "HoaDons",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMais_QuanLyMaQuanLy",
                table: "KhuyenMais",
                column: "QuanLyMaQuanLy");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamGioHangs_MaSanPham",
                table: "SanPhamGioHangs",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaHang",
                table: "SanPhams",
                column: "MaHang");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaKhuyenMai",
                table: "SanPhams",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaMauSac",
                table: "SanPhams",
                column: "MaMauSac");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietKhuyenMais");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "SanPhamGioHangs");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "GioHangs");

            migrationBuilder.DropTable(
                name: "Hangs");

            migrationBuilder.DropTable(
                name: "KhuyenMais");

            migrationBuilder.DropTable(
                name: "MauSacs");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "QuanLies");
        }
    }
}
