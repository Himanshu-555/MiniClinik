using System.ComponentModel.DataAnnotations;

namespace MiniClinik.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        public string phone { get; set; } = string.Empty;
        [Required]
        public int age { get; set; } = 0;
        [Required]
        public string gender { get; set; } = string.Empty;
        [Required, Compare(nameof(ConfirmPassword))]
        public string password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
