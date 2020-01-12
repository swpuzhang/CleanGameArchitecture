using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyNetQTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IBus _bus;

        public WeatherForecastController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task Get()
        {
            var msg = new Order() { OrderId = 1 };
            await _bus.PublishAsync(msg);

        }
    }
}
