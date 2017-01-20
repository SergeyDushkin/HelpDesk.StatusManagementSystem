using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public interface IStatusSourceRepository
    {        
        Task<IEnumerable<StatusSource>> GetAllAsync();
        Task<StatusSource> GetAsync(Guid id);
        Task<StatusSource> GetAsync(string name);
        Task AddAsync(StatusSource statusSource);
        Task UpdateAsync(StatusSource statusSource);
    }
}