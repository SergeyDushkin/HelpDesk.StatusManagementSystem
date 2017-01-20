using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Services
{
    public interface IStatusSourceService
    {
        Task<IEnumerable<StatusSource>> GetAllAsync();
        Task<StatusSource> GetAsync(Guid id);
        Task<StatusSource> GetAsync(string name);
        Task CreateAsync(string userId, string name, string description, DateTime createdAt);
    }
}