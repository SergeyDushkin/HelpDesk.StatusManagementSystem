using System;
using Coolector.Common.Types;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class BrowseEventStatus : PagedQueryBase
    {
        public Guid SourceId { get; set; }
        public Guid ReferenceId { get; set; }
    }
}