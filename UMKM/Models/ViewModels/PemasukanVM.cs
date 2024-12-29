namespace UMKM.Models.ViewModels
{
    public class PemasukanVM
    {
        public List<Pemasukan_harian> Harian { get; set; } = new List<Pemasukan_harian>();
        public List<string> Bulan { get; set; } = new List<string>();
    }
}