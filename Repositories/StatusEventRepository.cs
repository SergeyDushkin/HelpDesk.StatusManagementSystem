using System;
using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Types;
using servicedesk.StatusManagementSystem.Dal;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Repositories.Queries;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public class StatusEventRepository : IStatusEventRepository
    {        
        private readonly StatusDbContext _database;

        public StatusEventRepository(StatusDbContext database)
        {
            _database = database;
            _database.ChangeTracker.AutoDetectChangesEnabled = false;
        }
        
        public async Task<Maybe<PagedResult<StatusEvent>>> GetAsync(Guid referenceId)
            => await _database.StatusEvents.GetByReferanceIdAsync(referenceId);

        public IQueryable<StatusEvent> Get(Guid referenceId) => _database.StatusEvents.GetByReferanceId(referenceId);

        public async Task AddAsync(StatusEvent statusEvent) 
        {
            await _database.StatusEvents.AddAsync(statusEvent);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateAsync(StatusEvent statusEvent)
        {
            _database.StatusEvents.Update(statusEvent);
            await _database.SaveChangesAsync();
        }

    }
}