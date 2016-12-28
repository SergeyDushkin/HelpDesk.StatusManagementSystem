using System;
using System.Linq;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Repositories
{
    public interface IStatusRepository
    {        
        Task<IQueryable<Status>> GetAllAsync(Guid sourceId);
        Task<Status> GetAsync(Guid id);
        Task AddAsync(Status status);
        Task UpdateAsync(Status status);
    }
}