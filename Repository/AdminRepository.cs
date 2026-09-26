using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Clinic_Appointment_System.Data;
using Mini_Clinic_Appointment_System.DTO;
using Mini_Clinic_Appointment_System.Helper;

namespace Mini_Clinic_Appointment_System.Repository
{
    public class AdminRepository: IAdminRepository
    {
        private readonly MiniClinikContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public AdminRepository(MiniClinikContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public async Task<(bool success, string message)> CreateDoctorAsync(DoctorDTO createDoctor)
        {
            var existing = await context.Doctors.AsNoTracking()
                .FirstOrDefaultAsync(x => x.email == createDoctor.email);
            if (existing != null)
                return (false, "Doctor already exists with this email");

            var user = new ApplicationUser
            {
                UserName = createDoctor.email,
                Email = createDoctor.email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, createDoctor.Password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await roleManager.RoleExistsAsync("Doctor"))
                await roleManager.CreateAsync(new IdentityRole("Doctor"));

            await userManager.AddToRoleAsync(user, "Doctor");

            var doctor = mapper.Map<Doctor>(createDoctor);
            doctor.UserId = user.Id;
            doctor.isActive = true;

            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            return (true, "Doctor created successfully");
        }

        public async Task<List<AdminViewDoctorDTO>> ViewDoctorsDetailsAsync()
        {
            var doctors = await context.Doctors.AsNoTracking().ToListAsync();
            return mapper.Map<List<AdminViewDoctorDTO>>(doctors);
        }

        public async Task<List<AdminViewPatientDTO>> ViewPatientsDetailsAsync()
        {
            var patients = await context.Patients.AsNoTracking().ToListAsync();
            return mapper.Map<List<AdminViewPatientDTO>>(patients);
        }

        public async Task<(bool success, string message)> RemoveDoctorAsync(int doctorId)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(x => x.Id == doctorId);
            if (doctor == null)
                return (false, "Doctor not found");

            doctor.isActive = false;
            await context.SaveChangesAsync();
            return (true, "Doctor removed successfully");
        }
    }
}

