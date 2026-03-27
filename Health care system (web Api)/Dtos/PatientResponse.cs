using Health_care_system__web_Api_.Entities;

namespace Health_care_system__web_Api_.Dtos
{
    public class PatientResponse
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime BirthDate { get; set; }
            public List<AppointmentPatientResponse> Appointments { get; set; }
        
    }
}
