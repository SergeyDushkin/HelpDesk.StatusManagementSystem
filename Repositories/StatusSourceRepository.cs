using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
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

        public async Task<Maybe<PagedResult<StatusSource>>> GetAllAsync()
            => await _database.StatusSources.GetAllAsync();
        
        public async Task<Maybe<StatusSource>> GetAsync(Guid id)
            => await _database.StatusSources.GetByIdAsync(id);
        public async Task<Maybe<StatusSource>> GetAsync(string name)
            => await _database.StatusSources.GetByNameAsync(name);

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