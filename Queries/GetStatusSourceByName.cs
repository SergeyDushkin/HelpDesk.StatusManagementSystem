using Collectively.Common.Queries;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class GetStatusSourceByName : IQuery
    {
        public string Name { get; set; }
    }
}