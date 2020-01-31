using System;
using System.Threading.Tasks;
using MassTransit.Common;
using Serilog;

namespace MassTransit.Consumer
{
    public class DoSomethingConsumer2 : IConsumer<SomeModel2>
    {
        private readonly ILogger _log;

        public DoSomethingConsumer2(ILogger log)
        {
            _log = log;
        }

        public async Task Consume(ConsumeContext<SomeModel2> context)
        {
            var model = context.Message;
            _log.Error($"{this.GetType().Name}");
            Console.WriteLine($"{this.GetType().Name} is consumered.");
            Console.WriteLine($"{model.Id}|{model.Name}");
        }
    }
}
