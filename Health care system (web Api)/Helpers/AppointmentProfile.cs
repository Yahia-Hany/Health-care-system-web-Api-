using AutoMapper;
using Health_care_system__web_Api_.Dtos;
using Health_care_system__web_Api_.Entities;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentResponse>()
            .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.Name))
            .ForMember(dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient.Name));
    }
}