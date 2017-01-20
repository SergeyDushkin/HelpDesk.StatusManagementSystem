using System;
using System.Threading.Tasks;
using System.Linq;
using Coolector.Common.Extensions;
using servicedesk.StatusManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace servicedesk.StatusManagementSystem.Repositories.Queries
{
    public static class StatusQueries
    {
        public static async Task<IEnumerable<Status>> GetAllAsync(this IQueryable<Status> statuses, Guid sourceId)
        {
            return await statuses.Where(r => r.SourceId == sourceId).OrderBy(r => r.Step).ThenBy(r => r.Order).ToListAsync();
        }

        public static async Task<Status> GetByIdAsync(this IQueryable<Status> statuses, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await statuses.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}