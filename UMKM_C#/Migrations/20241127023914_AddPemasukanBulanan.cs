using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMKM_C_.Migrations
{
    /// <inheritdoc />
    public partial class AddPemasukanBulanan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Pengeluaran_bulanan_Pengeluaran_harian_HarianId",
            //    table: "Pengeluaran_bulanan");

            //migrationBuilder.DropIndex(
            //    name: "IX_Pengeluaran_bulanan_HarianId",
            //    table: "Pengeluaran_bulanan");

            //migrationBuilder.AddColumn<int>(
            //    name: "Pengeluaran_harianId",
            //    table: "Pengeluaran_bulanan",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Pemasukan_harian",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Total = table.Column<int>(type: "int", nullable: false),
            //        Created_at = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Updated_at = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Pemasukan_harian", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pemasukan_bulanan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HarianId = table.Column<int>(type: "int", nullable: false),
                    Bulan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pemasukan_bulanan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pemasukan_bulanan_Pemasukan_harian_HarianId",
                        column: x => x.HarianId,
                        principalTable: "Pemasukan_harian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Pengeluaran_bulanan_Pengeluaran_harianId",
            //    table: "Pengeluaran_bulanan",
            //    column: "Pengeluaran_harianId");

            migrationBuilder.CreateIndex(
                name: "IX_Pemasukan_bulanan_HarianId",
                table: "Pemasukan_bulanan",
                column: "HarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pengeluaran_harianId",
                table: "Pengeluaran_bulanan",
                column: "HarianId",
                principalTable: "Pengeluaran_harian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pengeluaran_harianId",
                table: "Pengeluaran_bulanan");

            migrationBuilder.DropTable(
                name: "Pemasukan_bulanan");

            //migrationBuilder.DropTable(
            //    name: "Pemasukan_harian");

            migrationBuilder.DropIndex(
                name: "IX_Pengeluaran_bulanan_Pengeluaran_harianId",
                table: "Pengeluaran_bulanan");

            migrationBuilder.DropColumn(
                name: "Pengeluaran_harianId",
                table: "Pengeluaran_bulanan");

            migrationBuilder.CreateIndex(
                name: "IX_Pengeluaran_bulanan_HarianId",
                table: "Pengeluaran_bulanan",
                column: "HarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pengeluaran_HarianId",
                table: "Pengeluaran_bulanan",
                column: "HarianId",
                principalTable: "Pengeluaran_harian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
