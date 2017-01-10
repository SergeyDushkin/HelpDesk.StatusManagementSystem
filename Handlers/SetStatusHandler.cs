using System.Threading.Tasks;
using Coolector.Common.Commands;
using Coolector.Common.Services;
using RawRabbit;
using servicedesk.Common.Commands;
using servicedesk.StatusManagementSystem.Events;
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
                .OnCustomError((ex, logger) => logger.Error(ex, "Error when trying to set new status. "))
                .OnError((ex, logger) => logger.Error(ex, "Error when trying to set new status."))
                .ExecuteAsync();
        }
    }
}

/*
await _handler
    .Run(async () => await _statusManager.SetNewAsync(command.Email, command.Token, command.Password))
    .OnSuccess(async () => await _bus.PublishAsync(new NewPasswordSet(command.Request.Id, command.Email)))
    .OnCustomError(async ex => await _bus.PublishAsync(new SetNewPasswordRejected(command.Request.Id, ex.Code, ex.Message, command.Email)))
    .OnError(async (ex, logger) =>
    {
        logger.Error(ex, "Error when trying to set new password.");
        await _bus.PublishAsync(new SetNewPasswordRejected(command.Request.Id,
            OperationCodes.Error, "Error when trying to set new password.", command.Email));
    })
    .ExecuteAsync()
*/