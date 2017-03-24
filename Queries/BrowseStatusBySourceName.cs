using Collectively.Common.Types;

namespace servicedesk.StatusManagementSystem.Queries
{
    public class BrowseStatusBySourceName : PagedQueryBase
    {
        public string Name { get;set; }
    }
}