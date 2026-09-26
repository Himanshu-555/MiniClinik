using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Clinic_Appointment_System.Model;
using Mini_Clinic_Appointment_System.Repository;

namespace Mini_Clinic_Appointment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> RegisterPatient([FromBody] SignUpModel model)
        {
            var result = await authRepository.RegisterAsync(model, "patient");
            if (!result.success)
               return BadRequest(result.message);
            return Ok(result.message);
        }
        [HttpPost("admin/signup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] SignUpModel model)
        {
            var result = await authRepository.RegisterAsync(model, "Admin");
            if (!result.success)
                return BadRequest(result.message);
            return Ok(result.message);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginPatient([FromBody] SignInModel model)
        {
            var token = await authRepository.LoginAsync(model, "patient");
            if (token == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new { token });
        }
        
        [HttpPost("admin/login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAdmin([FromBody] SignInModel model)
        {
            var token = await authRepository.LoginAsync(model, "Admin"); 
            if (token == null)
                return Unauthorized("Invalid email or password.");
            return Ok(new { token  });
        }
        [HttpPost("doctor/login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginDoctor([FromBody] SignInModel model)
        {
            var token = await authRepository.LoginAsync(model, "Doctor");
            if (token == null)
                return Unauthorized("Invalid email or password.");
            return Ok(new { token });
        }
    }
}
