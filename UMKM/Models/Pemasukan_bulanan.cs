using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UMKM.Models
{
    public class Pemasukan_bulanan
    {
        public int Id { get; set; }
        [ValidateNever]
        public Pemasukan_harian Pemasukan_harian { get; set; }
        public int HarianId { get; set; }
        public int Bulan { get; set; }
    }
}
