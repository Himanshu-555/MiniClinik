using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Clinic_Appointment_System.Data;
using Mini_Clinic_Appointment_System.DTO;
using Mini_Clinic_Appointment_System.Repository;
using System.Security.Claims;

namespace Mini_Clinic_Appointment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            this.doctorRepository = doctorRepository;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null) {
                return Unauthorized();
            }
            var doctor = await doctorRepository.GetProfileAsync(userID);
            if (!doctor.success)
            {
                return NotFound("profile not found");
            }
            return Ok(doctor.dto);
        }
        [HttpPatch("update-info")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateDoctorProfile updatedoctor)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
            {
                return Unauthorized();
            }
            var result = await doctorRepository.UpdateProfileAsync(updatedoctor,userID);
            if (!result.success)
            {
                return NotFound(result.msg);
            }
            return Ok(result.msg);
        }
        [HttpPatch("set-availability")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> SetAvailability([FromBody] UpdateDoctorTimeDTO updateAvailability)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userID == null)
            {
                return Unauthorized();
            }
            var result = await doctorRepository.SetAvailabiltyAsync(updateAvailability,userID);
            if (!result.success)
                return NotFound(result.success);
            return Ok(result.success);
        }
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> ViewAppointments()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
            {
                return Unauthorized();
            }
            var result = await doctorRepository.ViewAllAppointmetAsync(userID);
            if (!result.success)
            {
                return NotFound("appointment not found");
            }
            return Ok(result.appointments);
        }
        [HttpPost("update-appointment-status")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateAppointmentStatus([FromBody] UpdateAppointMentStatus updateStatus)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
            {
                return Unauthorized();
            }
            var result = await doctorRepository.UpdateStatusAsync(userID,updateStatus.appointmentId,updateStatus.status);
            if (!result.success)
                return NotFound(result.message);

            return Ok(result.message);
        }
        [HttpPost("add-prescription")]
        [Authorize(Roles = "Doctor")]
        public void AddPrescription()
        {

        }
    }
}
