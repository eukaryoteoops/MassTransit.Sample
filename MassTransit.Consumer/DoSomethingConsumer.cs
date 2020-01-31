using System;
using System.Threading.Tasks;
using MassTransit.Common;
using Serilog;

namespace MassTransit.Consumer
{
    public class DoSomethingConsumer : IConsumer<SomeModel>
    {
        private readonly ILogger _log;

        public DoSomethingConsumer(ILogger log)
        {
            _log = log;
        }

        public async Task Consume(ConsumeContext<SomeModel> context)
        {
            var model = context.Message;
            _log.Error($"{this.GetType().Name}");
            Console.WriteLine($"{this.GetType().Name} is consumered.");
            Console.WriteLine($"{model.Id}|{model.Name}");
        }
    }
}
