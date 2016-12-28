using System;
using System.Threading.Tasks;
using System.Linq;
using Coolector.Common.Extensions;
using servicedesk.StatusManagementSystem.Dal;
using servicedesk.StatusManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Coolector.Common.Types;

namespace servicedesk.StatusManagementSystem.Repositories.Queries
{
    public static class StatusEventQueries
    {
        public static IQueryable<StatusEvent> StatusEvents(this StatusDbContext database)
            => database.StatusEvents.AsNoTracking().AsQueryable();

        public static async Task<Maybe<PagedResult<StatusEvent>>> GetByReferanceIdAsync(this IQueryable<StatusEvent> statusEvents, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await Task.FromResult(statusEvents.AsNoTracking().Include(x => x.Status).Where(x => x.ReferenceId == id).PaginateWithoutLimit());
        }
        public static IQueryable<StatusEvent> GetByReferanceId(this IQueryable<StatusEvent> statusEvents, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return statusEvents.AsNoTracking().Include(x => x.Status).Where(x => x.ReferenceId == id);
        }
    }
}