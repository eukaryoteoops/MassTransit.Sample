using System;
using System.Threading.Tasks;

namespace MassTransit.Producer
{
    public class PublishObserver : IPublishObserver
    {
        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            Console.WriteLine("------------------PostPublish--------------------");
            var message = context.Message;
            Console.WriteLine("-------------------------------------------------");
            return Task.CompletedTask;
        }

        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            Console.WriteLine("------------------PrePublish--------------------");
            var message = context.Message;
            Console.WriteLine("------------------------------------------------");

            return Task.CompletedTask;
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            Console.WriteLine("------------------PublishFault--------------------");
            var message = context.Message;
            Console.WriteLine("--------------------------------------------------");

            return Task.CompletedTask;
        }
    }
}
