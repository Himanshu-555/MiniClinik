namespace Mini_Clinic_Appointment_System.DTO
{
    public class DoctorDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string Password { get; set; }
        public string phone { get; set; }
        public string specialist { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
    }
    public class UpdateDoctorProfile
    {
        public string name { get; set; }
        public string phone { get; set; }
        public int age { get; set; }

    }
    public class UpdateDoctorTimeDTO
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class ShowDoctorInfoToPatient
    {
        public string name { get; set; }
        public string specialist { get; set; }
        public string gender { get; set; }
    }
    public class Prescription
    {

    }
}
