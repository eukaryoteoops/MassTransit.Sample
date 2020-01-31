using System.Threading.Tasks;
using MassTransit.Common;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly IBus _bus;

        public ProducerController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            await _bus.Publish<SomeModel>(new { Id = 1, Name = "me" });
            await _bus.Publish<SomeModel2>(new { Id = 2, Name = "me2" });
            //await _bus.Publish(new SomeModel { Id = 1, Name = "me" });
            //var test = new Test(new int[] { 111, 222, 333, 444, 555 });
            //var a = test[0];
            return Ok();
        }
    }

    public class Test
    {
        private readonly int[] _array;
        public Test(int[] arr)
        {
            _array = arr;
        }
        public int this[int index] { get { return _array[index]; } }
    }
}
