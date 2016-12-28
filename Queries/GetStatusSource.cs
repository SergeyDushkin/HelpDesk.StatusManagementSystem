using System;
using Coolector.Common.Queries;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class GetStatusSource : IQuery
    {
        public Guid Id { get; set; }
    }
}