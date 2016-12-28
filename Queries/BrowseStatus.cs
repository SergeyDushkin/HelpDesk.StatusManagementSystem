using System;
using Coolector.Common.Types;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class BrowseStatus : PagedQueryBase
    {
        public Guid SourceId { get;set; }
    }
}