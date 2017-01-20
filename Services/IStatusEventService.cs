using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Services
{
    public interface IStatusEventService
    {
        Task<IEnumerable<StatusEvent>> GetAsync(Guid referenceId);
        Task<StatusEvent> GetCurrentAsync(Guid referenceId);
        Task CreateAsync(Guid referenceId, Guid sourceId, Guid statusId, string userId, string message, DateTime createdAt);
        Task RejectAsync(Guid referenceId, string code, string message);
        Task CompleteAsync(Guid referenceId, string message = null);
    }
}