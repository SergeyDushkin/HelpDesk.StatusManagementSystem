using System;

namespace servicedesk.StatusManagementSystem.Dto
{
    public class StatusEventDto
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public Guid StatusSourceId { get; set; }
        public Guid StatusId { get; set; }
        public string UserId { get; set; }
        public string State { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public bool IsApproved { get; set; }
        public bool IsUndo { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Success => State == "completed" && Code == "success";
        public StatusDto Status { get; set; }

    }
}