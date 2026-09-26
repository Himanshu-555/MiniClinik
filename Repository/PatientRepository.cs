using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mini_Clinic_Appointment_System.Data;
using Mini_Clinic_Appointment_System.DTO;
using Mini_Clinic_Appointment_System.Helper;

namespace Mini_Clinic_Appointment_System.Repository
{
    public class PatientRepository: IPatientRepository
    {
        private readonly MiniClinikContext context;
        private readonly IMapper mapper;

        public PatientRepository(MiniClinikContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<(bool success, PatientDTO profileDto)> GetProfileAsync(string userId)
        {
            var profile = await context.Patients.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            if (profile == null)
                return (false, null);

            var profileDTO = mapper.Map<PatientDTO>(profile);
            return (true, profileDTO);
        }
        public async Task<(bool success, string message)> UpdateProfileAsync(PatientDTO updatePatient, string userId)
        {
            var profile = await context.Patients.FirstOrDefaultAsync(x => x.UserId == userId);
            if (profile == null)
                return (false, "Profile not fetchd");
            mapper.Map(updatePatient, profile);
            await context.SaveChangesAsync();
            return (true, "updated successfully");
        }
        public async Task<List<ShowDoctorInfoToPatient>> ViewDoctorsAsync()
        {
            var doctor = await context.Doctors.AsNoTracking()
                .Where(d => d.isActive)
                .Select(
                d=> new ShowDoctorInfoToPatient
                {
                    name = d.name,
                    specialist = d.specialist,
                    gender = d.gender,
                }).ToListAsync();
            return doctor;
        }
        public async Task<(bool success, string msg, AppointmentResponseDTO appointment)> BookAppointMentAsync(BookAppointmentDTO dto,string userId)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                return (false, "Patient profile not found.", null);
            var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorId && d.isActive);
            if(doctor == null)
            {
                return (false, "No Doctor found",null);
            }
            if (dto.AppointMentDate.Date < DateTime.Now.Date)
            {
                return (false, "You can't book at old Date",null);
            }
            
            bool isAlreadyBooked = await context.Appointments.AnyAsync(d => d.DoctorId == dto.DoctorId && d.AppointmentDate == dto.AppointMentDate && d.Status != "Cancelled" );
            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = patient.Id,
                AppointmentDate = dto.AppointMentDate,
                Status = "pending",
                ProblemDescription = dto.ProblemDescription
            };
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();
            var response = new AppointmentResponseDTO
            {
                AppointmentId = appointment.Id,
                DoctorName = doctor.name,
                Specialist = doctor.specialist,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status
            };
            return (true, "Appointment booked successfully", response);

        }
        public async Task<(bool success, List<PatientViewAppointDetailDTO> appointments)> ViewAllAppointMentAsync(string userId)
        {
            var patient = await context.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                return (false, null);
            var appointments = await context.Appointments.AsNoTracking()
                .Where(a => a.PatientId == patient.Id)
                .Join(context.Doctors.AsNoTracking(),
                    a => a.DoctorId,
                    d => d.Id,
                    (a, d) => new PatientViewAppointDetailDTO
                    {
                        doctorName = d.name,
                        specialist = d.specialist,
                        gender = d.gender,
                        AppointmentDate = a.AppointmentDate,
                        Status = a.Status
                    })
                .ToListAsync();

            return (true, appointments);
        }


    }
}
