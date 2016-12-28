using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Services
{
    public interface IStatusEventService
    {
        Task<Maybe<PagedResult<StatusEvent>>> GetAsync(Guid referenceId);
        Task<Maybe<StatusEvent>> GetCurrentAsync(Guid referenceId);
        Task CreateAsync(Guid referenceId, Guid sourceId, Guid statusId, string userId, string message, DateTime createdAt);
        Task RejectAsync(Guid referenceId, string code, string message);
        Task CompleteAsync(Guid referenceId, string message = null);
    }
}