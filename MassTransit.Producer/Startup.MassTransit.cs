using System;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Producer
{
    public partial class Startup
    {
        public void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(o =>
            {
                o.AddBus(provider =>
                    Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(
                            new Uri(Configuration.GetValue<string>("Queue:Host")),
                            settings =>
                            {
                                settings.Username(Configuration.GetValue<string>("Queue:Username"));
                                settings.Password(Configuration.GetValue<string>("Queue:Password"));
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
            return TaskUtil.Await(async () =>
            {
                BusHandle busHandle = await bus.StartAsync();
                await busHandle.Ready;
                return busHandle;
            });
        }
    }
}
