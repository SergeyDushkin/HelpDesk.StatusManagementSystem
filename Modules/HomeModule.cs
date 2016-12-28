using Nancy;

namespace servicedesk.StatusManagementSystem.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => Response.AsJson(new { name = "servicedesk.StatusManagementSystem" }));
        }
    }
}