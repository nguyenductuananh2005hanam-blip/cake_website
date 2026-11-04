using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnhHoaBakery.Migrations
{
    /// <inheritdoc />
    public partial class TaoDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANHMUC",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANHMUC", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "DONHANG",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(15,0)", nullable: false),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDTNhan = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChiGiao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongThucTT = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DONHANG", x => x.MaDonHang);
                });

            migrationBuilder.CreateTable(
                name: "THANHVIEN",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THANHVIEN", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "BANH",
                columns: table => new
                {
                    MaBanh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false),
                    TenBanh = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(15,0)", nullable: false),
                    NgaySanXuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANH", x => x.MaBanh);
                    table.ForeignKey(
                        name: "FK_BANH_DANHMUC_MaDanhMuc",
                        column: x => x.MaDanhMuc,
                        principalTable: "DANHMUC",
                        principalColumn: "MaDanhMuc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHITIETDONHANG",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaBanh = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(15,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIETDONHANG", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_CHITIETDONHANG_BANH_MaBanh",
                        column: x => x.MaBanh,
                        principalTable: "BANH",
                        principalColumn: "MaBanh",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHITIETDONHANG_DONHANG_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DONHANG",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BANH_MaDanhMuc",
                table: "BANH",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETDONHANG_MaBanh",
                table: "CHITIETDONHANG",
                column: "MaBanh");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETDONHANG_MaDonHang",
                table: "CHITIETDONHANG",
                column: "MaDonHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIETDONHANG");

            migrationBuilder.DropTable(
                name: "THANHVIEN");

            migrationBuilder.DropTable(
                name: "BANH");

            migrationBuilder.DropTable(
                name: "DONHANG");

            migrationBuilder.DropTable(
                name: "DANHMUC");
        }
    }
}
