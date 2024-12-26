using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UMKM.Models
{
    public class Pemasukan_harian
    {
        public int Id { get; set; }
        [Required]
        public int Total { get; set; }
        public string? Created_at { get; set; }
        public string? Updated_at { get; set; }
        [ValidateNever]
        public ICollection<Pemasukan_bulanan> Pemasukan_bulanan { get; set; }
    }
}
