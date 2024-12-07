namespace UMKM_C_.Models.ViewModels
{
    public class PengeluaranViewModel
    {
        public List<Pengeluaran_harian> Pengeluaran_Harian { get; set; } = new List<Pengeluaran_harian>();
        public List<Pengeluaran_bulanan> Pengeluaran_Bulanan { get; set; } = new List<Pengeluaran_bulanan>();
    }
}
