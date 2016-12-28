using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Services
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAllAsync(Guid sourceId);
        Task<Status> GetAsync(Guid id);
        Task CreateAsync(Guid sourceId, string name, string description, int order, int step, bool isFinal, StatusSource source, DateTime createdAt);
    }
}