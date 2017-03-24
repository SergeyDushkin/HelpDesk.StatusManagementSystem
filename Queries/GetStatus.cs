using System;
using Collectively.Common.Queries;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class GetStatus : IQuery
    {
        public Guid Id { get; set; }
    }
}