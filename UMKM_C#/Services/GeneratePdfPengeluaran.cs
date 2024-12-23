using QuestPDF.Fluent;
using QuestPDF.Helpers;
using UMKM_C_.Models;

namespace UMKM_C_.Services
{
    public class GeneratePdfPengeluaran
    {
        public GeneratePdfPengeluaran()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }

        public byte[] GeneratePengeluaran(List<Pengeluaran_bulanan> data)
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
                            foreach (var item in data)
                            {
                                table.Cell().Border(1).AlignCenter().Text((i + 1).ToString());
                                table.Cell().Border(1).Text(item.Pengeluaran_harian.Nama);
                                table.Cell().Border(1).AlignRight().Text(item.Pengeluaran_harian.Jumlah.ToString());
                                table.Cell().Border(1).AlignRight().Text(item.Pengeluaran_harian.Harga.ToString("C")); // Format currency
                                i++;
                            }
                            table.Cell().ColumnSpan(3).Border(1).Text("Total").AlignCenter().Bold();
                            table.Cell().Border(1).AlignRight().Text(data.Sum(p => p.Pengeluaran_harian.Harga).ToString("C"));
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }
    }
}