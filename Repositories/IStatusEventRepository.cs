using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public interface IStatusEventRepository
    {        
        Task<IEnumerable<StatusEvent>> GetAsync(Guid referenceId);
        Task<StatusEvent> GetCurrentByReferenceIdAsync(Guid referenceId) ;
        Task AddAsync(StatusEvent statusEvent);
        Task UpdateAsync(StatusEvent statusEvent);
    }
}