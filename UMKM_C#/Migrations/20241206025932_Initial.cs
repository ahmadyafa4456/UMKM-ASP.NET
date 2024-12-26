using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMKM.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Bahan",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        nama = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        jumlah = table.Column<int>(type: "int", nullable: false),
            //        harga = table.Column<int>(type: "int", nullable: false),
            //        created_at = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Bahan", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

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

            //migrationBuilder.CreateTable(
            //    name: "Pengeluaran_harian",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Nama = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Harga = table.Column<int>(type: "int", nullable: false),
            //        Jumlah = table.Column<int>(type: "int", nullable: false),
            //        Created_at = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Updated_at = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Pengeluaran_harian", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Pemasukan_bulanan",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        HarianId = table.Column<int>(type: "int", nullable: false),
            //        Bulan = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Pemasukan_bulanan", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Pemasukan_bulanan_Pemasukan_harian_HarianId",
            //            column: x => x.HarianId,
            //            principalTable: "Pemasukan_harian",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pengeluaran_bulanan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HarianId = table.Column<int>(type: "int", nullable: false),
                    Bulan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pengeluaran_bulanan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pengeluaran_bulanan_Pengeluaran_harian_HarianId",
                        column: x => x.HarianId,
                        principalTable: "Pengeluaran_harian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Pemasukan_bulanan_HarianId",
            //    table: "Pemasukan_bulanan",
            //    column: "HarianId");

            migrationBuilder.CreateIndex(
                name: "IX_Pengeluaran_bulanan_HarianId",
                table: "Pengeluaran_bulanan",
                column: "HarianId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bahan");

            migrationBuilder.DropTable(
                name: "Pemasukan_bulanan");

            migrationBuilder.DropTable(
                name: "Pengeluaran_bulanan");

            migrationBuilder.DropTable(
                name: "Pemasukan_harian");

            migrationBuilder.DropTable(
                name: "Pengeluaran_harian");
        }
    }
}
