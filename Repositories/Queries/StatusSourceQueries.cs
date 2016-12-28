using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Repositories.Queries
{
    public static class StatusSourceQueries
    {
        public static async Task<Maybe<PagedResult<StatusSource>>> GetAllAsync(this IQueryable<StatusSource> statusSources)
        {
            return await Task.FromResult(statusSources.AsQueryable().Paginate(1, 100));
        }
        public static async Task<StatusSource> GetByIdAsync(this IQueryable<StatusSource> statusSources, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await statusSources.SingleOrDefaultAsync(x => x.Id == id);
        }
        public static async Task<StatusSource> GetByNameAsync(this IQueryable<StatusSource> statusSources, string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            return await statusSources.SingleOrDefaultAsync(x => x.Name == name);
        }
    }
}