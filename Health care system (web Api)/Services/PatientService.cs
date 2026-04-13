using Health_care_system__web_Api_.Entities;
using Health_care_system__web_Api_.Rrpositories;

namespace Health_care_system__web_Api_.Services
{
    public class PatientService(IGenericRepository<Patient> repository) : IPatientService
    {
        public Task<Patient> CreateAsync(Patient entity)
            => repository.CreateAsync(entity);

        public Task<bool> DeleteAsync(int id)
            => repository.DeleteAsync(id);

        public async Task<IEnumerable<Patient>> GetAllAsync()
            => await repository.GetAllAsync();

        public Task<Patient> GetByIdAsync(int id)
            => repository.GetByIdAsync(id);

        public Task<bool> UpdateAsync(Patient entity)
            => repository.UpdateAsync(entity);
    }
}
