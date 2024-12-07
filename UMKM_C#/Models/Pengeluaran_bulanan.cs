using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UMKM_C_.Models
{
    public class Pengeluaran_bulanan
    {
        public int Id { get; set; }
        [ValidateNever]
        public int HarianId { get; set; }
        public Pengeluaran_harian Pengeluaran_harian { get; set; }
        public int Bulan { get; set; }
    }
}
