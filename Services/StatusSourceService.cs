using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
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

        public async Task<Maybe<PagedResult<StatusSource>>> GetAllAsync()
            => await _statusSourceRepository.GetAllAsync();

        public async Task<Maybe<StatusSource>> GetAsync(Guid id)
            => await _statusSourceRepository.GetAsync(id);
        public async Task<Maybe<StatusSource>> GetAsync(string name)
            => await _statusSourceRepository.GetAsync(name);

        public async Task CreateAsync(string userId, string name, string description, DateTime createdAt)
        {
            var statusEvent = new StatusSource(userId, name, description, createdAt);
            await _statusSourceRepository.AddAsync(statusEvent);
        }

        private async Task UpdateAsync(Guid id, Action<StatusSource> update)
        {
            var statusEvent = await _statusSourceRepository.GetAsync(id);
            if (statusEvent.HasNoValue)
                return;

            update(statusEvent.Value);
            await _statusSourceRepository.UpdateAsync(statusEvent.Value);
        }
    }
}