using AutoMapper;
using Mini_Clinic_Appointment_System.Data;
using Mini_Clinic_Appointment_System.DTO;

namespace Mini_Clinic_Appointment_System.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Doctor, DoctorDTO>();
            CreateMap<Patient, PatientDTO>().ReverseMap();   // patient update ke liye reverse bhi chahiye
            CreateMap<UpdateDoctorTimeDTO, Doctor>();
            CreateMap<UpdateDoctorProfile, Doctor>();        // doctor profile update ke liye

            // Admin module ke liye naye maps
            CreateMap<DoctorDTO, Doctor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.isActive, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            CreateMap<Doctor, AdminViewDoctorDTO>();
            CreateMap<Patient, AdminViewPatientDTO>();
        }
    }
}
