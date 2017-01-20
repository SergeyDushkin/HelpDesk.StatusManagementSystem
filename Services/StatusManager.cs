using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Services
{
    public class StatusManager : IStatusManager
    {
        private readonly IStatusService _statusService;
        private readonly IStatusEventService _statusEventService;

        public StatusManager(IStatusService statusService, IStatusEventService statusEventService)
        {
            _statusService = statusService;
            _statusEventService = statusEventService;
        }

        public async Task<IEnumerable<Status>> GetNextStatuses(Guid sourceId, Guid referenceId)
        {
            var currentEvent = await _statusEventService.GetCurrentAsync(referenceId);
            var statuses = await _statusService.GetAllAsync(sourceId);

            if (currentEvent == null)
            {
                return statuses.Where(r => r.Step == 1).OrderBy(r => r.Order); 
            }

            if (currentEvent.Status.IsFinal) 
            {
                return default(IEnumerable<Status>);
            }

            var step = currentEvent.Status.Step + 1;

            return statuses.Where(r => r.Step == step).OrderBy(r => r.Order);
        }

        public async Task SetNextStatusAsync(Guid sourceId, Guid referenceId, Guid statusId, string userId, string message)
        {
            var allowed = await GetNextStatuses(sourceId, referenceId);

            if (!allowed.Select(r => r.Id).Contains(statusId))
                throw new Exception("Not valid status id");

            await _statusEventService.CreateAsync(referenceId, sourceId, statusId, userId, message, DateTime.Now);
        }
    }
}