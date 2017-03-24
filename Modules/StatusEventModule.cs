using AutoMapper;
using Collectively.Common.Extensions;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Dto;
using servicedesk.StatusManagementSystem.Queries;
using servicedesk.StatusManagementSystem.Services;

namespace servicedesk.StatusManagementSystem.Modules
{
    public class StatusEventModule : ModuleBase
    {
        public StatusEventModule(IStatusEventService statusEventService, IMapper mapper) 
            : base(mapper, "statusEvents")
        {
            Get("{referenceId}", args => FetchCollection<BrowseEventStatus, StatusEvent>
                (async x => (await statusEventService.GetAsync(x.ReferenceId)).PaginateWithoutLimit())
                .MapTo<StatusEventDto>()
                .HandleAsync());

            /*
            Get("{referenceId}", async args => {
                var referenceId = (Guid)args.referenceId;
     
                var q = await statusEventService.GetAsync(referenceId);
                var result = q.HasValue;

                if (result) {
                    var dto = mapper.Map<StatusEventDto>(q.Value);
                    return Response.AsJson(dto);
                }

                return Response.AsJson(result);
            });*/
        }
    }
}