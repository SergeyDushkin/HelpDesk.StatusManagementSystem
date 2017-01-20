using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Repositories;

namespace servicedesk.StatusManagementSystem.Services
{
    public class StatusEventService : IStatusEventService
    {
        private readonly IStatusEventRepository _statusEventRepository;

        public StatusEventService(IStatusEventRepository statusEventRepository)
        {
            _statusEventRepository = statusEventRepository;
        }

        public Task<IEnumerable<StatusEvent>> GetAsync(Guid referenceId)
            => _statusEventRepository.GetAsync(referenceId);
            
        public Task<StatusEvent> GetCurrentAsync(Guid referenceId) 
            => _statusEventRepository.GetCurrentByReferenceIdAsync(referenceId);

        public async Task CreateAsync(Guid referenceId, Guid sourceId, Guid statusId, string userId, string message, DateTime createdAt)
        {
            var statusEvent = new StatusEvent(referenceId, sourceId, statusId, userId, createdAt);
            statusEvent.SetMessage(message);
            statusEvent.SetCode("200");
            
            await _statusEventRepository.AddAsync(statusEvent);
        }

        public async Task RejectAsync(Guid referenceId, string code, string message)
            => await UpdateAsync(referenceId, x => x.Reject(code, message));

        public async Task CompleteAsync(Guid referenceId, string message = null)
            => await UpdateAsync(referenceId, x => x.Complete(message));

        private async Task UpdateAsync(Guid referenceId, Action<StatusEvent> update)
        {
            await Task.FromResult(0);
            throw new NotImplementedException();
        }
    }
}