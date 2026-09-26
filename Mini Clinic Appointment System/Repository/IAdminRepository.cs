using Mini_Clinic_Appointment_System.DTO;

namespace Mini_Clinic_Appointment_System.Repository
{
    public interface IAdminRepository
    {
        Task<(bool success, string message)> CreateDoctorAsync(DoctorDTO createDoctor);
        Task<List<AdminViewDoctorDTO>> ViewDoctorsDetailsAsync();
        Task<List<AdminViewPatientDTO>> ViewPatientsDetailsAsync();
        Task<(bool success, string message)> RemoveDoctorAsync(int doctorId);
    }
}
