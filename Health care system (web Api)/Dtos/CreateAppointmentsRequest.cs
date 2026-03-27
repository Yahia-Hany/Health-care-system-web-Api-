using Health_care_system__web_Api_.Entities;

namespace Health_care_system__web_Api_.Dtos
{
    public class CreateAppointmentsRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
