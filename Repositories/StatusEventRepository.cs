using System;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Dal;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Repositories.Queries;
using NLog;
using System.Collections.Generic;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public class StatusEventRepository : IStatusEventRepository
    {        
        private readonly StatusDbContext _database;
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public StatusEventRepository(StatusDbContext database)
        {
            _database = database;
            _database.ChangeTracker.AutoDetectChangesEnabled = false;
        }
        
        public Task<IEnumerable<StatusEvent>> GetAsync(Guid referenceId)
            => _database.StatusEvents.GetByReferanceIdAsync(referenceId);

        public Task<StatusEvent> GetCurrentByReferenceIdAsync(Guid referenceId) 
            => _database.StatusEvents.GetCurrentByReferanceIdAsync(referenceId);

        public async Task AddAsync(StatusEvent statusEvent) 
        {
            Logger.Info("Add Status event: {0}", Newtonsoft.Json.JsonConvert.SerializeObject(statusEvent));

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