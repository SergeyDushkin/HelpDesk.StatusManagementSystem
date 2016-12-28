using System;
using Coolector.Common.Commands;

namespace servicedesk.StatusManagementSystem.Commands
{
    public class SetStatus : ICommand
    {
        public Request Request { get; set; }
        public Guid SourceId { get; set; }
        public Guid ReferenceId { get; set; }
        public Guid StatusId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}