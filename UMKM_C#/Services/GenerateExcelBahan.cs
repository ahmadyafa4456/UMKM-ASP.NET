using OfficeOpenXml;
using UMKM_C_.Models;

namespace UMKM_C_.Services
{
    public class GenerateExcelBahan
    {
        public GenerateExcelBahan()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public byte[] GenerateBahan(List<Bahan> bahan)
        {
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("BahanReport");

                // Transform data sebelum dimuat
                var transformedData = bahan.Select((p, index) => new
                {
                    No = index + 1,
                    Nama = p.nama,
                    Tanggal = p.created_at,
                    Harga = p.harga
                }).ToList();

                // Muat data ke worksheet
                var range = ws.Cells["A1"].LoadFromCollection(transformedData, true);

                // Sesuaikan lebar kolom
                range.AutoFitColumns();

                // Format header
                var headerRow = ws.Cells[1, 1, 1, range.Columns];
                headerRow.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerRow.Style.Font.Bold = true;

                // Format semua data menjadi tengah
                var dataRows = ws.Cells[2, 1, transformedData.Count + 1, range.Columns];
                dataRows.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Tambahkan total harga di bawah tabel
                var totalRowIndex = transformedData.Count + 2;
                ws.Cells[totalRowIndex, 1].Value = "Total Harga";
                ws.Cells[totalRowIndex, 1, totalRowIndex, range.Columns - 1].Merge = true; // Gabungkan kolom
                ws.Cells[totalRowIndex, 1, totalRowIndex, range.Columns - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[totalRowIndex, range.Columns].Formula = $"SUM(D2:D{transformedData.Count + 1})"; // Kolom harga

                // Format total baris
                var totalRow = ws.Cells[totalRowIndex, 1, totalRowIndex, range.Columns];
                totalRow.Style.Font.Bold = true;
                totalRow.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Kembalikan file sebagai byte array
                return package.GetAsByteArray();
            }
        }
    }
}