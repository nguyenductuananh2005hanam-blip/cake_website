using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnhHoaBakery.Migrations
{
    /// <inheritdoc />
    public partial class ThemBangCuaHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CUAHANG",
                columns: table => new
                {
                    MaCuaHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCuaHang = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Hotline = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AnhMinhHoa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUAHANG", x => x.MaCuaHang);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CUAHANG");
        }
    }
}
