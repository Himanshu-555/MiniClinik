using System.ComponentModel.DataAnnotations;

namespace Mini_Clinic_Appointment_System.Model
{
    public class SignInModel
    {
        [Required, EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
