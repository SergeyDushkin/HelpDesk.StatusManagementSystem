using System;
using Collectively.Common.Domain;

namespace servicedesk.StatusManagementSystem.Domain
{
    public class Status : IdentifiableEntity, ITimestampable
    {
        public Guid SourceId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Order { get; protected set; }
        public int Step { get; protected set; }
        public bool IsFinal { get; protected set; }
        public StatusSource Source { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected Status()
        {
        }

        public Status(Guid sourceId, string name, string description, int order, int step, bool isFinal, StatusSource source, DateTime createdAt)
        {
            SourceId = sourceId;
            Name = name;
            Description = description;
            Order = order;
            Step = step;
            IsFinal = isFinal;
            Source = source;
            CreatedAt = createdAt;
        }
    }
}