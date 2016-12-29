using System;
using Coolector.Common.Events;

namespace servicedesk.StatusManagementSystem.Events
{
    public class NextStatusSet : IEvent
    {
        public Guid RequestId { get; }
        public Guid SourceId { get; }
        public Guid ReferenceId { get; }
        public Guid StatusId { get; }

        protected NextStatusSet()
        {
        }

        public NextStatusSet(Guid requestId, Guid sourceId, Guid referenceId, Guid statusId)
        {
            RequestId = requestId; 
            SourceId = sourceId; 
            ReferenceId = referenceId;
            StatusId = statusId;
        }
    }
}