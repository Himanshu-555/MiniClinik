using Mini_Clinic_Appointment_System.Data;

namespace Mini_Clinic_Appointment_System.DTO
{
    public class AppointMentDTO
    {

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
    public class PatientViewAppointDetailDTO
    {
        public string doctorName { get; set; }
        public string specialist { get; set; }
        public string gender { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } 

    }
    public class DoctorViewAppointDetailDTO
    {
        public string patientName { get; set; }
        public int age { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string ProblemDescription { get; set; }

    }
    public class UpdateAppointMentStatus
    {
        public int appointmentId { get; set; }
        public string status { get; set; }
    }
}
