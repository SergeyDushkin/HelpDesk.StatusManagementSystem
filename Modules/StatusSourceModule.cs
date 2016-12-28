using AutoMapper;
using Coolector.Common.Extensions;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Dto;
using servicedesk.StatusManagementSystem.Queries;
using servicedesk.StatusManagementSystem.Services;

namespace servicedesk.StatusManagementSystem.Modules
{
    public class StatusSourceModule : ModuleBase
    {
        public StatusSourceModule(IStatusSourceService statusSourceService, IStatusService statusService, IStatusEventService statusEventService, IStatusManager statusManager, IMapper mapper) 
            : base(mapper, "sources")
        {
            Get("", args => FetchCollection<BrowseStatusSource, StatusSource>
                (async x => await statusSourceService.GetAllAsync())
                .MapTo<StatusSourceDto>()
                .HandleAsync());

            Get("{id:guid}", args => Fetch<GetStatusSource, StatusSource>
                (async x => await statusSourceService.GetAsync(x.Id))
                .MapTo<StatusSourceDto>()
                .HandleAsync());

            Get("{name}", args => Fetch<GetStatusSourceByName, StatusSource>
                (async x => await statusSourceService.GetAsync(x.Name))
                .MapTo<StatusSourceDto>()
                .HandleAsync());

            Get("{sourceId:guid}/statuses", args => FetchCollection<BrowseStatus, Status>
                (async x => (await statusService.GetAllAsync(x.SourceId)).PaginateWithoutLimit())
                .MapTo<StatusDto>()
                .HandleAsync());

            Get("{name}/statuses", args => FetchCollection<BrowseStatusBySourceName, Status>
                (async x => {
                    var source = await statusSourceService.GetAsync(x.Name);
                    return (await statusService.GetAllAsync(source.Value.Id)).PaginateWithoutLimit();
                })
                .MapTo<StatusDto>()
                .HandleAsync());

            Get("{sourceId:guid}/events/{referenceId:guid}", args => FetchCollection<BrowseEventStatus, StatusEvent>
                (async x => await statusEventService.GetAsync(x.ReferenceId))
                .MapTo<StatusEventDto>()
                .HandleAsync());

            Get("{sourceId:guid}/events/{referenceId:guid}/current", args => Fetch<GetStatusEvent, StatusEvent>
                (async x => await statusEventService.GetCurrentAsync(x.ReferenceId))
                .MapTo<StatusEventDto>()
                .HandleAsync());

            Get("{sourceId:guid}/events/{referenceId:guid}/next", args => FetchCollection<BrowseEventStatus, Status>
                (async x => (await statusManager.GetNextStatuses(x.SourceId, x.ReferenceId)).PaginateWithoutLimit())
                .MapTo<StatusDto>()
                .HandleAsync());
        }
    }
}