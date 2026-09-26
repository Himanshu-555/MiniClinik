using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mini_Clinic_Appointment_System.DTO;
using Mini_Clinic_Appointment_System.Repository;
using System.Security.Claims;

namespace Mini_Clinic_Appointment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var result = await patientRepository.GetProfileAsync(userId);

            if (!result.success)
                return NotFound("profile not found");
            return Ok(result.profileDto);
        }
        [HttpPatch("update-info")]
        [Authorize(Roles ="patient")]
        public async Task<IActionResult> UpdateProfile([FromBody] PatientDTO updatePatient)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var result = await patientRepository.UpdateProfileAsync(updatePatient, userId);
            if (!result.success)
                return NotFound(result.message);
            return Ok(result.message);
        }
        [HttpPost("book-appointment")]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> BookAppointMent([FromBody] BookAppointmentDTO bookAppointment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var result = await patientRepository.BookAppointMentAsync(bookAppointment, userId);
            if (!result.success)
                return BadRequest(result.msg);
            return Ok(new { message = result.msg, appointment = result.appointment });
        }
        [HttpGet("view-appointment")]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> ViewAppointMent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var result = await patientRepository.ViewAllAppointMentAsync(userId);
            if (!result.success)
                return NotFound("profile not found");
            return Ok(result.appointments);
        }
        [HttpGet("view-doctor")]
        public async Task<IActionResult> ViewDoctors()
        {
            var result = await patientRepository.ViewDoctorsAsync();
            return Ok(result);
        }
    }
}
