using System;
using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Types;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public interface IStatusEventRepository
    {        
        Task<Maybe<PagedResult<StatusEvent>>> GetAsync(Guid referenceId);
        IQueryable<StatusEvent> Get(Guid referenceId);
        Task AddAsync(StatusEvent statusEvent);
        Task UpdateAsync(StatusEvent statusEvent);
    }
}