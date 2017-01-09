using System;
using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Types;
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

        public async Task<Maybe<PagedResult<StatusEvent>>> GetAsync(Guid referenceId)
            => await _statusEventRepository.GetAsync(referenceId);
        public async Task<Maybe<StatusEvent>> GetCurrentAsync(Guid referenceId) 
            => await Task.FromResult(_statusEventRepository.Get(referenceId).Where(r => !r.IsUndo).OrderByDescending(r => r.Date).FirstOrDefault());

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
            throw new NotImplementedException();
        }
    }
}