using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit.AspNetCoreIntegration;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Sample
{
    public partial class Startup
    {
        public void ConfigureIoC(IServiceCollection services)
        {
            services.AddMassTransit(o =>
            {
                //add consumer into container.
                //o.AddConsumer<DoSomethingConsumer>();
                //add all consumer into container.
                o.AddConsumers(Assembly.GetExecutingAssembly());
                o.AddBus(provider =>
                    Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(
                            new Uri("rabbitmq://127.0.0.1"),
                            settings =>
                            {
                                settings.Username("guest");
                                settings.Password("guest");
                            });
                        cfg.ReceiveEndpoint("queue", x =>
                        {
                            x.UseMessageRetry(y => y.Interval(2, 100));
                            //mapping consumer to endpoint.
                            x.Consumer<DoSomethingConsumer>(services.BuildServiceProvider());
                            x.Consumer<DoSomething2Consumer>(services.BuildServiceProvider());
                        });
                    }));
            });
        }
    }

    public static class MassTransitExtension
    {
        public static BusHandle StartBusHandle(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetService<IBusControl>();
            // publish decorator
            bus.ConnectPublishObserver(new PublishObserver());
            // consume decorator
            bus.ConnectConsumeObserver(new ConsumeObserver());
            return TaskUtil.Await(async () =>
            {
                BusHandle busHandle = await bus.StartAsync();
                await busHandle.Ready;
                return busHandle;
            });
        }
    }
}
