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
using RabbitMQ.Client.Exceptions;
using System.IO;
using Polly;
using System;
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

            container.Update(builder =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<StatusDbContext>();
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("StatusDatabase"));
                var db = new StatusDbContext(optionsBuilder.Options);

                builder.RegisterInstance(db).InstancePerRequest().SingleInstance();

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
}