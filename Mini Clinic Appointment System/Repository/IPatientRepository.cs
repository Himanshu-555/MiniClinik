using Mini_Clinic_Appointment_System.DTO;

namespace Mini_Clinic_Appointment_System.Repository
{
    public interface IPatientRepository
    {
        Task<(bool success, PatientDTO profileDto)> GetProfileAsync(string userId);
        Task<(bool success, string message)> UpdateProfileAsync(PatientDTO updatePatient, string userId);
        Task<List<ShowDoctorInfoToPatient>> ViewDoctorsAsync();
        Task<(bool success, string msg, AppointmentResponseDTO appointment)> BookAppointMentAsync(BookAppointmentDTO dto, string userId);
        Task<(bool success, List<PatientViewAppointDetailDTO> appointments)> ViewAllAppointMentAsync(string userId);
    }
}
