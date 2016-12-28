using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public interface IStatusSourceRepository
    {        
        Task<Maybe<PagedResult<StatusSource>>> GetAllAsync();
        Task<Maybe<StatusSource>> GetAsync(Guid id);
        Task<Maybe<StatusSource>> GetAsync(string name);
        Task AddAsync(StatusSource statusSource);
        Task UpdateAsync(StatusSource statusSource);
    }
}