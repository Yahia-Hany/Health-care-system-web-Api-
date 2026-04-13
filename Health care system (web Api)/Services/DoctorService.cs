using Health_care_system__web_Api_.Entities;
using Health_care_system__web_Api_.Rrpositories;

namespace Health_care_system__web_Api_.Services
{
    public class DoctorService(IGenericRepository<Doctor> repository) : IDoctorService
    {
        public Task<Doctor> CreateAsync(Doctor entity)
            => repository.CreateAsync(entity);

        public Task<bool> DeleteAsync(int id)
            => repository.DeleteAsync(id);

        public async Task<IEnumerable<Doctor>> GetAllAsync()
            => await repository.GetAllAsync();

        public Task<Doctor> GetByIdAsync(int id)
            => repository.GetByIdAsync(id);

        public Task<bool> UpdateAsync(Doctor entity)
            => repository.UpdateAsync(entity);
    }
}
