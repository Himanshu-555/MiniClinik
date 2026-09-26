namespace Mini_Clinic_Appointment_System.Data
{
    public class Appointment
    {
        public int Id { get; set; }

        // Foreign Keys
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        // Appointment Metadata
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Pending"; 
        public string ProblemDescription { get; set; }
    }
}
