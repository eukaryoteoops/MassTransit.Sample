using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Sample
{
    public class DoSomething2Consumer : IConsumer<SomeModel>
    {
        public async Task Consume(ConsumeContext<SomeModel> context)
        {
            var model = context.Message;
            Console.WriteLine($"{this.GetType().Name} is consumered.");
            Console.WriteLine($"{model.Id}|{model.Name}");
        }
    }
}
