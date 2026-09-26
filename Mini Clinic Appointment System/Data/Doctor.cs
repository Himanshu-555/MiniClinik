using Mini_Clinic_Appointment_System.Helper;
using System.ComponentModel.DataAnnotations;

namespace Mini_Clinic_Appointment_System.Data
{
    public class Doctor
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string specialist { get; set; }
        public int age { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string gender { get; set; }
        public bool isActive { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser applicationUser;
        public ICollection<Appointment> Appointments { get; set; }
    }
}
