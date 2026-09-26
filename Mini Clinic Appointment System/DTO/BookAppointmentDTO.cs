namespace Mini_Clinic_Appointment_System.DTO
{
    public class BookAppointmentDTO
    {
        public string name { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointMentDate { get; set; }
        public string? ProblemDescription { get; set; }
    }
    public class AppointmentResponseDTO
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string Specialist { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
    }
}
