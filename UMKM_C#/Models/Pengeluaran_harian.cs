using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UMKM_C_.Models
{
    public class Pengeluaran_harian
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "nama tidak boleh kosong")]
        [StringLength(50)]
        public string Nama { get; set; }
        [Required(ErrorMessage = "harga tidak boleh kosong")]
        public int? Harga { get; set; }
        [Required(ErrorMessage = "jumlah tidak boleh kosong")]
        public int? Jumlah { get; set; }
        public string? Created_at { get; set; }
        public string? Updated_at { get; set; }
        [ValidateNever]
        public ICollection<Pengeluaran_bulanan> Pengeluaran_bulanan { get; set; }
    }
}
