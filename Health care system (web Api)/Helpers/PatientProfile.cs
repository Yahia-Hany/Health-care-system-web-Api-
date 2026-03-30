using AutoMapper;
using Health_care_system__web_Api_.Dtos;
using Health_care_system__web_Api_.Entities;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<Patient, PatientResponse>();

        CreateMap<CreatePatientsRequest, Patient>();

        CreateMap<UpdatePatientsRequest, Patient>();
        CreateMap<Appointment, AppointmentDoctorResponse>()
            .ForMember(dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient.Name));
    }
}