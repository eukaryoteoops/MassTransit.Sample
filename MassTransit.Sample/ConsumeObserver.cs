using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Sample
{
    public class ConsumeObserver : IConsumeObserver
    {
        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            Console.WriteLine("------------------ConsumeFault--------------------");
            var message = context.Message;
            Console.WriteLine("--------------------------------------------------");
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            Console.WriteLine("------------------PostConsume--------------------");
            var message = context.Message;
            Console.WriteLine("-------------------------------------------------");
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            Console.WriteLine("------------------PreConsume--------------------");
            var message = context.Message;
            Console.WriteLine("------------------------------------------------");
            return Task.CompletedTask;
        }
    }
}
