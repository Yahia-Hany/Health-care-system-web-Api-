namespace Health_care_system__web_Api_.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public string Specialization { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
