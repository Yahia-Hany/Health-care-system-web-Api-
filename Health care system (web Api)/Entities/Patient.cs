namespace Health_care_system__web_Api_.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
