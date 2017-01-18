using servicedesk.Common.Commands;
using servicedesk.Common.Host;
using servicedesk.StatusManagementSystem.Framework;

namespace servicedesk.StatusManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10010)
                .UseAutofac(Bootstrapper.LifeTimeScope)
                .UseRabbitMq()
                .SubscribeToCommand<SetStatus>(exchangeName: "servicedesk.statusmanagementsystem.commands", routingKey : "setstatus.job")
                .Build()
                .Run();
        }
    }
}
