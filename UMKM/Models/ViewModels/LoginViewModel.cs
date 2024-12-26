using System.ComponentModel.DataAnnotations;

namespace UMKM.ViewModels.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username harus lebih dari 3 hingga 50 karakter")]
        public string UserName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Password harus lebih dari 3 hingga 50 karakter")]

        public string Password { get; set; }
    }
}