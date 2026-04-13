using Health_care_system__web_Api_.Data;
using Health_care_system__web_Api_.Entities;
using Microsoft.EntityFrameworkCore;
namespace Health_care_system__web_Api_.Rrpositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext) 
        {
           _dbContext = dbContext;
        }
        public async Task<T> CreateAsync(T entity)
        { 
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);


        public async Task<bool> UpdateAsync(T entity)
        {
            var model = await GetByIdAsync(entity.Id);
            if (model == null)
                return false;
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}
