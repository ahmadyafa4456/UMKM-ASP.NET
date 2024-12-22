using QuestPDF.Fluent;
using QuestPDF.Helpers;
using UMKM_C_.Models;

namespace UMKM_C_.Services
{
    public class GeneratePdfPemasukan
    {
        public GeneratePdfPemasukan()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }

        public byte[] GeneratePemasukan(List<Pemasukan_bulanan> pemasukan)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.Header().Height(60).AlignCenter().AlignMiddle().Text("Total Pemasukan Bulanan")
                    .FontSize(20).Bold().FontFamily("Arial");

                    page.Content().PaddingTop(30).Column(column =>
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Border(1).Text("#").Bold().AlignCenter();
                                header.Cell().Border(1).Text("Tanggal").Bold();
                                header.Cell().Border(1).Text("Pemasukan").AlignRight().Bold();
                            });

                            int i = 0;
                            foreach (var item in pemasukan)
                            {
                                table.Cell().Border(1).AlignCenter().Text((i + 1).ToString());
                                table.Cell().Border(1).Text(item.Pemasukan_harian.Created_at);
                                table.Cell().Border(1).AlignRight().Text(item.Pemasukan_harian.Total.ToString("C")); // Format currency
                                i++;
                            }

                            table.Cell().ColumnSpan(2).Border(1).Text("Total").AlignCenter().Bold();
                            table.Cell().Border(1).AlignRight().Text(pemasukan.Sum(p => p.Pemasukan_harian.Total).ToString("C"));
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }
    }
}