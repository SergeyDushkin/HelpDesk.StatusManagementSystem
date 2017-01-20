using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Repositories;

namespace servicedesk.StatusManagementSystem.Services
{
    public class StatusSourceService : IStatusSourceService
    {
        private readonly IStatusSourceRepository _statusSourceRepository;

        public StatusSourceService(IStatusSourceRepository statusSourceRepository)
        {
            _statusSourceRepository = statusSourceRepository;
        }

        public Task<IEnumerable<StatusSource>> GetAllAsync()
            => _statusSourceRepository.GetAllAsync();

        public Task<StatusSource> GetAsync(Guid id)
            => _statusSourceRepository.GetAsync(id);
            
        public Task<StatusSource> GetAsync(string name)
            => _statusSourceRepository.GetAsync(name);

        public async Task CreateAsync(string userId, string name, string description, DateTime createdAt)
        {
            var statusEvent = new StatusSource(userId, name, description, createdAt);
            await _statusSourceRepository.AddAsync(statusEvent);
        }

        private async Task UpdateAsync(Guid id, Action<StatusSource> update)
        {
            var statusEvent = await _statusSourceRepository.GetAsync(id);
            if (statusEvent == null)
                return;

            update(statusEvent);
            await _statusSourceRepository.UpdateAsync(statusEvent);
        }
    }
}