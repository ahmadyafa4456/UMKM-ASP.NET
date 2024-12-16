namespace UMKM_C_.Services
{
    public class Generate
    {
        public GenerateExcelBahan excelBahan;
        public GeneratePdfBahan pdfBahan;
        public Generate()
        {
            this.excelBahan = new GenerateExcelBahan();
            this.pdfBahan = new GeneratePdfBahan();
        }
    }
}