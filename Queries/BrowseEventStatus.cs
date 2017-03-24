using System;
using Collectively.Common.Types;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class BrowseEventStatus : PagedQueryBase
    {
        public string Name { get; set; }
        public Guid SourceId { get; set; }
        public Guid ReferenceId { get; set; }
    }
}