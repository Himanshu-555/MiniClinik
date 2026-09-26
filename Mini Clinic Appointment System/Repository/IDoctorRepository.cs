using Mini_Clinic_Appointment_System.DTO;

namespace Mini_Clinic_Appointment_System.Repository
{
    public interface IDoctorRepository
    {
        Task<(bool success, DoctorDTO dto)> GetProfileAsync(string userID);
        Task<(bool success, string msg)> UpdateProfileAsync(UpdateDoctorProfile updateDoctor, string userID);
        Task<(bool success, List<DoctorViewAppointDetailDTO> appointments)> ViewAllAppointmetAsync(string userID);
        Task<(bool success, string message)> UpdateStatusAsync(string userID, int appointMentId, string status);
        Task<(bool success, string message)> SetAvailabiltyAsync(UpdateDoctorTimeDTO updateTime, string userID);
    }
}
