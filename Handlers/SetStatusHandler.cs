using System.Threading.Tasks;
using Coolector.Common.Services;
using RawRabbit;
using servicedesk.Common.Commands;
using servicedesk.Common.Events;
using servicedesk.StatusManagementSystem.Services;

namespace servicedesk.StatusManagementSystem.Handlers
{
    public class SetStatusHandler : ICommandHandler<SetStatus>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IStatusManager _statusManager;
        private readonly IStatusSourceService _statusSourceService;

        public SetStatusHandler(IHandler handler, 
            IBusClient bus, 
            IStatusManager statusManager,
            IStatusSourceService statusSourceService)
        {
            _handler = handler;
            _bus = bus;
            _statusManager = statusManager;
            _statusSourceService = statusSourceService;
        }

        public async Task HandleAsync(SetStatus command)
        {
            //var source = await _statusSourceService.GetAsync(command.SourceName);

            await _handler
                .Run(async () => await _statusManager.SetNextStatusAsync(command.SourceId, command.ReferenceId, command.StatusId, command.UserId, command.Message))
                .OnSuccess(async () => await _bus.PublishAsync(new NextStatusSet(command.Request.Id, command.SourceId, command.ReferenceId, command.StatusId)))
                .OnCustomError(async (ex, logger) => await _bus.PublishAsync(new SetNewStatusRejected(command.Request.Id, "error", "Error when trying to set new status.")))
                .OnError(async (ex, logger) => 
                {
                    logger.Error(ex, "Error when trying to set new status.");
                    await _bus.PublishAsync(new SetNewStatusRejected(command.Request.Id, "error", "Error when trying to set new status."));
                })
                .ExecuteAsync();
        }
    }
}