using AutoMapper;
using Collectively.Common.Nancy;

namespace servicedesk.StatusManagementSystem.Modules
{
    public abstract class ModuleBase : ApiModuleBase
    {
        protected ModuleBase() { }

        protected ModuleBase(string modulePath) 
            : base(modulePath) { }

        protected ModuleBase(IMapper mapper, string modulePath = "")
            : base(mapper, modulePath) { }
    }
}