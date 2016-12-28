using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Services
{
    public interface IStatusManager
    {
        Task<IEnumerable<Status>> GetNextStatuses(Guid sourceId, Guid referenceId);
        Task SetNextStatusAsync(Guid sourceId, Guid referenceId, Guid statusId, string userId, string message);
    }
}