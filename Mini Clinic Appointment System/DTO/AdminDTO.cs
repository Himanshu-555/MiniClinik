namespace Mini_Clinic_Appointment_System.DTO
{
    public class AdminDTO
    {

    }
    public class AdminViewDoctorDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string specialist { get; set; }
        public bool isActive { get; set; }
    }
    public class AdminViewPatientDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public bool isActive { get; set; }
    }
}
