using Mini_Clinic_Appointment_System.Model;

namespace Mini_Clinic_Appointment_System.Repository
{
    public interface IAuthRepository
    {
        Task<(bool success, string message)> RegisterAsync(SignUpModel model, string role);
        Task<string?> LoginAsync(SignInModel model, string requiredRole);
    }
}
