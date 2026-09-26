using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mini_Clinic_Appointment_System.Data;
using Mini_Clinic_Appointment_System.DTO;

namespace Mini_Clinic_Appointment_System.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly MiniClinikContext context;
        private readonly IMapper mapper;

        public DoctorRepository(MiniClinikContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<(bool success,DoctorDTO dto)> GetProfileAsync(string userID)
        {
            var doctor = await context.Doctors.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userID);
            if (doctor == null)
                return (false,null) ;
            var dto = mapper.Map<DoctorDTO>(doctor);
            return (true,dto);
        }
        public async Task<(bool success, string msg)> UpdateProfileAsync(UpdateDoctorProfile updateDoctor,string userID)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(x => x.UserId == userID);
            if (doctor == null)
            {
                return (false, "Doctor Not Found");
            }
            mapper.Map(updateDoctor,doctor);
            await context.SaveChangesAsync();
            return (true,"profile updated");
        }
        public async Task<(bool success, string message)> SetAvailabiltyAsync(UpdateDoctorTimeDTO updateTime,string userID)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(x => x.UserId == userID);
            if (doctor == null)
            {
                return (false, "unauthorized");
            }
            mapper.Map(updateTime,doctor);
            await context.SaveChangesAsync();
            return (true, "update successfully");
        }
        public async Task<(bool success, List<DoctorViewAppointDetailDTO> appointments)> ViewAllAppointmetAsync(string userID)
        {
            var doctor = await context.Doctors.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userID);
            if (doctor == null)
                return (false, null);
            var appointments = await context.Appointments.AsNoTracking()
                .Where(a => a.DoctorId == doctor.Id)
                .Join(context.Patients.AsNoTracking(),
                    a => a.PatientId,
                    p => p.Id,
                    (a, p) => new DoctorViewAppointDetailDTO
                    {
                        patientName = p.name,
                        age = p.age,
                        AppointmentDate = a.AppointmentDate,
                        Status = a.Status,
                        ProblemDescription = a.ProblemDescription
                    })
                .ToListAsync();
            return (true, appointments);
        }
        public async Task<(bool success, string message)> UpdateStatusAsync(string userID, int appointMentId,string status)
        {
            var doctor = await context.Doctors.AsNoTracking().FirstOrDefaultAsync(x=>x.UserId == userID);
            if (doctor == null)
                return (false, "unauthorized");
            var appointment = await context.Appointments.FirstOrDefaultAsync(x => x.Id == appointMentId && x.DoctorId == doctor.Id);
            if (appointment == null)
                return (false, "Appointment not found");
            appointment.Status = status;
            await context.SaveChangesAsync();
            return (true, "status updated");
        }
        
    }
}
