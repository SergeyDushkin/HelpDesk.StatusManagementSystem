using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using NLog;
using Coolector.Common.Services;
using Coolector.Common.Nancy;
using Coolector.Common.Extensions;
using Coolector.Common.Exceptionless;
using servicedesk.Common.Commands;
using servicedesk.Common.Events;
using servicedesk.StatusManagementSystem.Repositories;
using servicedesk.StatusManagementSystem.Services;
using servicedesk.StatusManagementSystem.Dal;
using Microsoft.EntityFrameworkCore;
using Nancy.Configuration;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;

namespace servicedesk.StatusManagementSystem.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IExceptionHandler _exceptionHandler;
        private readonly IConfiguration _configuration;

        public static ILifetimeScope LifeTimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Configure(INancyEnvironment environment)
        {
            environment.Tracing(enabled: true, displayErrorTraces: true);
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            Logger.Info("Coolector.Services.Storage Configuring application container");
            base.ConfigureApplicationContainer(container);

            var rmqRetryPolicy = Policy
                .Handle<ConnectFailureException>()
                .Or<BrokerUnreachableException>()
                .Or<IOException>()
                .WaitAndRetry(5, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => {
                        Logger.Error(exception, $"Cannot connect to RabbitMQ. retryCount:{retryCount}, duration:{timeSpan}");
                    }
                );

                /*
            var rmqRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => {
                        Logger.Error(exception, $"Cannot connect to RabbitMQ. retryCount:{retryCount}, duration:{timeSpan}");
                    }
                );*/

            container.Update(builder =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<StatusDbContext>();
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("StatusDatabase"));
                
                builder.RegisterType<StatusDbContext>().WithParameter("options", optionsBuilder.Options).AsSelf();

                builder.RegisterInstance(AutoMapperConfig.InitializeMapper());
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();

                builder.RegisterType<StatusEventRepository>().As<IStatusEventRepository>();
                builder.RegisterType<StatusEventService>().As<IStatusEventService>();

                builder.RegisterType<StatusSourceRepository>().As<IStatusSourceRepository>();
                builder.RegisterType<StatusSourceService>().As<IStatusSourceService>();

                builder.RegisterType<StatusRepository>().As<IStatusRepository>();
                builder.RegisterType<StatusService>().As<IStatusService>();

                builder.RegisterType<StatusManager>().As<IStatusManager>();
                
                builder.RegisterType<Handler>().As<IHandler>();
                /*
                var rawRabbitConfiguration = _configuration.GetSettings<RawRabbitConfiguration>();
                builder.RegisterInstance(rawRabbitConfiguration).SingleInstance();
                rmqRetryPolicy.Execute(() => builder
                        .RegisterInstance(new ConnectionFactory() { 
                            Port = rawRabbitConfiguration.Port, 
                            UserName = rawRabbitConfiguration.Username, 
                            Password = rawRabbitConfiguration.Password }
                        .CreateConnection(rawRabbitConfiguration.Hostnames).CreateModel())
                        .As<IModel>()
                );*/

                var rawRabbitConfiguration = _configuration.GetSettings<RawRabbitConfiguration>();
                builder.RegisterInstance(rawRabbitConfiguration).SingleInstance();
                rmqRetryPolicy.Execute(() => builder
                        .RegisterInstance(BusClientFactory.CreateDefault(rawRabbitConfiguration))
                        .As<IBusClient>()
                );

                var assembly = typeof(Startup).GetTypeInfo().Assembly;
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            });

            LifeTimeScope = container;
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                _exceptionHandler.Handle(ex, ctx.ToExceptionData(),
                    "Request details", "servicedesk", "Service", "StatusManagementSystem");

                return ctx.Response;
            });
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            //var databaseSettings = container.Resolve<MongoDbSettings>();
            //var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            //databaseInitializer.InitializeAsync();

            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,PUT,GET,OPTIONS,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers",
                    "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            Logger.Info("servicedesk.StatusManagementSystem API has started.");
        }
    }

    /*
    public class RawRabbitConfiguration
    {
        public static RawRabbitConfiguration Local { get; }
        public bool AutoCloseConnection { get; set; }
        public bool AutomaticRecovery { get; set; }
        public GeneralExchangeConfiguration Exchange { get; set; }
        public TimeSpan GracefulShutdown { get; set; }
        public List<string> Hostnames { get; set; }
        public string Password { get; set; }
        public bool PersistentDeliveryMode { get; set; }
        public int Port { get; set; }
        public TimeSpan PublishConfirmTimeout { get; set; }
        public GeneralQueueConfiguration Queue { get; set; }
        public TimeSpan RecoveryInterval { get; set; }
        public TimeSpan RequestTimeout { get; set; }
        public bool RouteWithGlobalId { get; set; }
        public SslOption Ssl { get; set; }
        public bool TopologyRecovery { get; set; }
        public string Username { get; set; }
        public string VirtualHost { get; set; }
    }

     public class GeneralExchangeConfiguration
    {
        public bool AutoDelete { get; set; }
        public bool Durable { get; set; }
        public ExchangeType Type { get; set; }
    }

    public class GeneralQueueConfiguration
    {
        public bool AutoDelete { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
    }
    
    public enum ExchangeType
    {
        Unknown = 0,
        Direct = 1,
        Fanout = 2,
        Headers = 3,
        Topic = 4
    }*/
}