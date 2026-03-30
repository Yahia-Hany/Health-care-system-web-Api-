using AutoMapper;
using Health_care_system__web_Api_.Entities;
using Health_care_system__web_Api_.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<Doctor, DoctorResponse>();
        CreateMap<CreateDoctorsRequest, Doctor>();
        CreateMap<UpdateDoctorRequest, Doctor>();
        CreateMap<Appointment, AppointmentDoctorResponse>()
         .ForMember(dest => dest.PatientName,
            opt => opt.MapFrom(src => src.Patient.Name));
    }
}