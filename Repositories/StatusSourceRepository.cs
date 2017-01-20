using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Dal;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Repositories.Queries;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public class StatusSourceRepository : IStatusSourceRepository
    {        
        private readonly StatusDbContext _database;

        public StatusSourceRepository(StatusDbContext database)
        {
            _database = database;
            _database.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public Task<IEnumerable<StatusSource>> GetAllAsync()
            => _database.StatusSources.GetAllAsync();
        
        public Task<StatusSource> GetAsync(Guid id)
            => _database.StatusSources.GetByIdAsync(id);

        public Task<StatusSource> GetAsync(string name)
            => _database.StatusSources.GetByNameAsync(name);

        public async Task AddAsync(StatusSource statusSource) 
        {
            await _database.StatusSources.AddAsync(statusSource);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateAsync(StatusSource statusSource)
        {
            _database.StatusSources.Update(statusSource);
            await _database.SaveChangesAsync();
        }

    }
}