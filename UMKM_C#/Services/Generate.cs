namespace UMKM_C_.Services
{
    public class Generate
    {
        public GenerateExcelBahan excelBahan;
        public GeneratePdfBahan pdfBahan;
        public GenerateExcelPemasukan excelPemasukan;
        public GeneratePdfPemasukan pdfPemasukan;
        public Generate()
        {
            this.excelBahan = new GenerateExcelBahan();
            this.pdfBahan = new GeneratePdfBahan();
            this.excelPemasukan = new GenerateExcelPemasukan();
            this.pdfPemasukan = new GeneratePdfPemasukan();
        }
    }
}