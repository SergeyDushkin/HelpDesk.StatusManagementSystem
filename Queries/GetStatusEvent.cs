using System;
using Collectively.Common.Queries;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class GetStatusEvent : IQuery
    {
        public Guid SourceId { get; set; }
        public Guid ReferenceId { get; set; }
    }
}