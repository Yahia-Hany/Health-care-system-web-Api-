using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Health_care_system__web_Api_.Entities;

namespace Health_care_system__web_Api_.Data.Interceptor
{
    public class CreatedAtInceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SetCreatedAt(eventData);
            return result;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            SetCreatedAt(eventData);
            return ValueTask.FromResult(result);
        }
        private static void SetCreatedAt(DbContextEventData eventData)
        {
            if (eventData.Context == null) return;
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is null || entry.State != EntityState.Added || !(entry.Entity is BaseEntity entity))
                    continue;

                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

            }
        }
    }
}
