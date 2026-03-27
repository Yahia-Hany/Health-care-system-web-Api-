namespace Health_care_system__web_Api_.Dtos
{
    public class DoctorResponse
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Specialization { get; set; }

            public List<AppointmentDoctorResponse> Appointments { get; set; }

    }
}
