using System;

namespace servicedesk.StatusManagementSystem.Dto
{
    public class StatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Step { get; set; }
        public bool IsFinal { get; set; }
    }
}