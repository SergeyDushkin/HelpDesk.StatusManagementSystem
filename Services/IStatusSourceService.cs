using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Services
{
    public interface IStatusSourceService
    {
        Task<Maybe<PagedResult<StatusSource>>> GetAllAsync();
        Task<Maybe<StatusSource>> GetAsync(Guid id);
        Task<Maybe<StatusSource>> GetAsync(string name);
        Task CreateAsync(string userId, string name, string description, DateTime createdAt);
    }
}