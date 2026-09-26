using System.ComponentModel.DataAnnotations;

namespace Mini_Clinic_Appointment_System.Model
{
    public class SignUpModel
    {
        [Required]
        public string name { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public int age { get; set; }
        [Required]
        public string gender { get; set; }
        [Required, Compare(nameof(ConfirmPassword))]
        public string password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
