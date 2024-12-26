using Microsoft.AspNetCore.Http.HttpResults;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UMKM.Models;

namespace UMKM.Services
{
    public class GeneratePdfBahan
    {
        public GeneratePdfBahan()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }
        public byte[] GenerateBahan(List<Bahan> bahan)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.Header().Height(60).AlignCenter().AlignMiddle().Text("Daftar Bahan")
            .FontSize(20)
            .Bold()
            .FontFamily("Arial");

                    page.Content().PaddingTop(30).Column(column =>
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40); // # column
                                columns.RelativeColumn(3); // Nama
                                columns.RelativeColumn(1); // Jumlah
                                columns.RelativeColumn(2); // Harga
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Border(1).Text("#").Bold().AlignCenter();
                                header.Cell().Border(1).Text("Nama").Bold();
                                header.Cell().Border(1).Text("Jumlah").AlignRight().Bold();
                                header.Cell().Border(1).Text("Harga").AlignRight().Bold();
                            });

                            // Data Bahan
                            int i = 0;
                            foreach (var item in bahan)
                            {
                                table.Cell().Border(1).AlignCenter().Text((i + 1).ToString());
                                table.Cell().Border(1).Text(item.nama);
                                table.Cell().Border(1).AlignRight().Text(item.jumlah.ToString());
                                table.Cell().Border(1).AlignRight().Text(item.harga.ToString("C")); // Format currency
                                i++;
                            }
                            table.Cell().ColumnSpan(3).Border(1).Text("Total").AlignCenter().Bold();
                            table.Cell().Border(1).AlignRight().Text(bahan.Sum(p => p.harga).ToString("C"));
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }

    }
}
