using Health_care_system__web_Api_.Entities;

namespace Health_care_system__web_Api_.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(int id);
        Task<Doctor> CreateAsync(Doctor entity);
        Task<bool> UpdateAsync(Doctor entity);
        Task<bool> DeleteAsync(int id);
    }
}
