namespace UMKM_C_.Models.ViewModels
{
    public class PemasukanViewModel
    {
        public Pemasukan_harian? Pemasukan_harian { get; set; }
        public List<Pemasukan_bulanan> Pemasukan_bulanan { get; set; } = new List<Pemasukan_bulanan>();
        public int Total;
    }
}
