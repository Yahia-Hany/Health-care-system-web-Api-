namespace Health_care_system__web_Api_.Dtos
{
    public class updateAppointmentsRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
