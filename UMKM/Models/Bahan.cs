using System.ComponentModel.DataAnnotations;

namespace UMKM.Models
{
    public class Bahan
    {
        [CsvHelper.Configuration.Attributes.Ignore]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Nama barang tidak boleh kosong")]
        [StringLength(50)]
        public string nama { get; set; }
        [Required(ErrorMessage = "Jumlah barang tidak boleh kosong")]
        public int jumlah { get; set; }
        [Required(ErrorMessage = "Harga barang tidak boleh kosong")]
        public int harga { get; set; }
        public string? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
