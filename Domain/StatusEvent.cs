using System;
using Coolector.Common.Domain;
using Coolector.Common.Extensions;

namespace servicedesk.StatusManagementSystem.Domain
{
    public class StatusEvent : IdentifiableEntity, ITimestampable
    {
        public Guid ReferenceId { get; protected set; }
        public Guid StatusSourceId { get; protected set; }
        public Guid StatusId { get; protected set; }
        public string UserId { get; protected set; }
        public string State { get; protected set; }
        public string Message { get; protected set; }
        public string Code { get; protected set; }
        public bool IsApproved { get; protected set; }
        public bool IsUndo { get; protected set; }
        public DateTimeOffset Date { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        public Status Status { get; protected set; }

        protected StatusEvent()
        {
        }

        public StatusEvent(Guid referenceId, Guid statusSourceId, Guid statusId, string userId, DateTime createdAt)
        {
            ReferenceId = referenceId;
            StatusSourceId = statusSourceId;
            StatusId = statusId;
            UserId = userId;
            CreatedAt = createdAt;
            State = States.Created;
            Date = DateTime.UtcNow;
            IsApproved = true;
            IsUndo = false;
        }

        public void Complete(string message = null)
        {
            if (State.EqualsCaseInvariant(States.Rejected))
                throw new InvalidOperationException($"StatusEvent: {Id} has been rejected and can not be completed.");

            SetCode("success"); //OperationCodes.Success
            SetMessage(message);
            SetState(States.Completed);
        }

        public void Reject(string code, string message)
        {
            if (State.EqualsCaseInvariant(States.Completed))
                throw new InvalidOperationException($"StatusEvent: {Id} has been completed and can not be rejected.");

            SetCode(code);
            SetMessage(message);
            SetState(States.Rejected);
        }

        public void SetMessage(string message)
        {
            if(Message.EqualsCaseInvariant(message))
                return;
            if (message?.Length > 500)
            {
                throw new ArgumentException("Message can not have more than 500 characters.",
                    nameof(message));
            }

            Message = message;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetState(string state)
        {
            if(State.EqualsCaseInvariant(state))
                return;

            State = state;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCode(string code)
        {
            if(Code.EqualsCaseInvariant(code))
                return;

            Code = code;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}