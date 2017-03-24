using System;
using Collectively.Common.Domain;

namespace servicedesk.StatusManagementSystem.Domain
{
    public class StatusSource : IdentifiableEntity, ITimestampable
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected StatusSource()
        {
        }

        public StatusSource(string userId, string name, string description, DateTime createdAt)
        {
            UserId = userId;
            Name = name;
            UserId = description;
            CreatedAt = createdAt;
        }
    }
}