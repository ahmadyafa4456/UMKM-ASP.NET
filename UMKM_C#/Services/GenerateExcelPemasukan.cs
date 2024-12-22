using OfficeOpenXml;
using UMKM_C_.Models;

namespace UMKM_C_.Services
{
    public class GenerateExcelPemasukan
    {
        public GenerateExcelPemasukan()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public byte[] GeneratePemasukan(List<Pemasukan_bulanan> pemasukan)
        {
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("PemasukanReport");
                var transformedData = pemasukan.Select((p, index) => new
                {
                    No = index + 1,
                    Tanggal = p.Pemasukan_harian.Created_at,
                    Pemasukan = p.Pemasukan_harian.Total
                }).ToList();

                var range = ws.Cells["A1"].LoadFromCollection(transformedData, true);
                range.AutoFitColumns();
                var headerRow = ws.Cells[1, 1, 1, range.Columns];
                headerRow.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerRow.Style.Font.Bold = true;

                var dataRow = ws.Cells[2, 1, transformedData.Count + 1, range.Columns];
                dataRow.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var totalIndex = transformedData.Count + 2;
                ws.Cells[totalIndex, 1].Value = "Total Pemasukan";
                ws.Cells[totalIndex, 1, totalIndex, range.Columns - 1].Merge = true;
                ws.Cells[totalIndex, 1, totalIndex, range.Columns - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[totalIndex, range.Columns].Formula = $"SUM(C2:C{transformedData.Count + 1})";

                var totalRow = ws.Cells[totalIndex, 1, totalIndex, range.Columns];
                totalRow.Style.Font.Bold = true;
                totalRow.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                return package.GetAsByteArray();
            }
        }
    }
}