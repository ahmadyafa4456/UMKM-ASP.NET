namespace UMKM_C_.Models
{
    public class Pemasukan
    {
        public List<Pemasukan_harian> pemasukan_Harian { get; set; } = new List<Pemasukan_harian>();
        public List<Pemasukan_bulanan> pemasukan_Bulanan { get; set; } = new List<Pemasukan_bulanan>();
    }
}