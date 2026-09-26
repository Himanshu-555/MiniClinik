using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Clinic_Appointment_System.DTO;
using Mini_Clinic_Appointment_System.Repository;

namespace Mini_Clinic_Appointment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }
        [HttpPost("create-doctor")]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorDTO doctor)
        {
            var result = await adminRepository.CreateDoctorAsync(doctor);
            if (!result.success)
                return BadRequest(result.message);
            return Ok(result.message);
        }

        [HttpGet("view-doctors")]
        public async Task<IActionResult> ViewDoctorsDetails()
        {
            var result = await adminRepository.ViewDoctorsDetailsAsync();
            return Ok(result);
        }

        [HttpGet("view-patients")]
        public async Task<IActionResult> ViewPatientsDetails()
        {
            var result = await adminRepository.ViewPatientsDetailsAsync();
            return Ok(result);
        }

        [HttpPatch("remove-doctor/{doctorId}")]
        public async Task<IActionResult> RemoveDoctor(int doctorId)
        {
            var result = await adminRepository.RemoveDoctorAsync(doctorId);
            if (!result.success)
                return NotFound(result.message);
            return Ok(result.message);
        }
    }

}

