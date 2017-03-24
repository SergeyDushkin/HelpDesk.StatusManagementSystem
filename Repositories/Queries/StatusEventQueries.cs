using System;
using System.Threading.Tasks;
using System.Linq;
using Collectively.Common.Extensions;
using servicedesk.StatusManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace servicedesk.StatusManagementSystem.Repositories.Queries
{
    public static class StatusEventQueries
    {
        //public static IQueryable<StatusEvent> StatusEvents(this StatusDbContext database)
        //    => database.StatusEvents.AsNoTracking().AsQueryable();

        public static async Task<IEnumerable<StatusEvent>> GetByReferanceIdAsync(this IQueryable<StatusEvent> statusEvents, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await statusEvents.Include(x => x.Status).Where(x => x.ReferenceId == id).ToListAsync();
        }
        public static async Task<StatusEvent> GetCurrentByReferanceIdAsync(this IQueryable<StatusEvent> statusEvents, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await statusEvents
                .Include(x => x.Status)
                .Where(x => x.ReferenceId == id)
                .Where(r => !r.IsUndo)
                .OrderByDescending(r => r.Date)
                .FirstOrDefaultAsync();
        }
    }
}