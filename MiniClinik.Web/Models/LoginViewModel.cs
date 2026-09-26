using System.ComponentModel.DataAnnotations;

namespace MiniClinik.Web.Models
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } 

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
