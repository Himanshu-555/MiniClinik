namespace MiniClinik.Web.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password, string role);
    }
}
