using Coolector.Common.Host;
using servicedesk.Common.Commands;
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
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<SetStatus>()
                .Build()
                .Run();
        }
    }
}
