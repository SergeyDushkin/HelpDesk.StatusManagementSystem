using System;
using System.Linq;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Dal;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Repositories.Queries;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public class StatusRepository : IStatusRepository
    {        
        private readonly StatusDbContext _database;

        public StatusRepository(StatusDbContext database)
        {
            _database = database;
            _database.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public async Task<IQueryable<Status>> GetAllAsync(Guid sourceId)
            => await _database.Statuses.GetAllAsync(sourceId);
        
        public async Task<Status> GetAsync(Guid id)
            => await _database.Statuses.GetByIdAsync(id);

        public async Task AddAsync(Status status) 
        {
            await _database.Statuses.AddAsync(status);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateAsync(Status status)
        {
            _database.Statuses.Update(status);
            await _database.SaveChangesAsync();
        }

    }
}