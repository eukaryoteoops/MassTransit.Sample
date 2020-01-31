using System;
using System.IO;
using System.Reflection;
using GreenPipes;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Filters;

namespace MassTransit.Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(o =>
            {
                var config = new LoggerConfiguration()
                .MinimumLevel.Error()
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .WriteTo.RollingFile($"{Directory.GetCurrentDirectory()}/Log/log-{{Date}}.txt");
                return config.CreateLogger();
            });
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
                            new Uri(Configuration.GetValue<string>("Queue:Host")),
                            settings =>
                            {
                                settings.Username(Configuration.GetValue<string>("Queue:Username"));
                                settings.Password(Configuration.GetValue<string>("Queue:Password"));
                            });
                        cfg.ReceiveEndpoint("test.queue", x =>
                        {
                            x.UseMessageRetry(y => y.Interval(2, 100));
                            //mapping consumer to endpoint.
                            x.Consumer<DoSomethingConsumer>(provider);
                        });
                        cfg.ReceiveEndpoint("test.queue2", x =>
                        {
                            x.UseMessageRetry(y => y.Interval(2, 100));
                            //mapping consumer to endpoint.
                            x.Consumer<DoSomethingConsumer2>(provider);
                        });
                    }));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            var bus = app.ApplicationServices.GetService<IBusControl>();
            // consume decorator
            bus.ConnectConsumeObserver(new ConsumeObserver());
            var busHandle = TaskUtil.Await(async () =>
            {
                BusHandle busHandle = await bus.StartAsync();
                await busHandle.Ready;
                return busHandle;
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                busHandle.Stop();
            });
        }
    }
}
