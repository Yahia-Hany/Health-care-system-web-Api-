using Health_care_system__web_Api_.Entities;

namespace Health_care_system__web_Api_.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(int id);
        Task<Patient> CreateAsync(Patient entity);
        Task<bool> UpdateAsync(Patient entity);
        Task<bool> DeleteAsync(int id);
    }
}
