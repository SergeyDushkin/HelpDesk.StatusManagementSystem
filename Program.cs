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
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<SetStatus>("setstatus")
                .Build()
                .Run();
        }
    }
}
