using GameMessages.MqCmds;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class HostedService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;

        public HostedService(IBusControl busControl, IConfiguration configuration)
        {
            _busControl = busControl;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken);
            var mqcfg = _configuration.GetSection("Rabbitmq");
            var gameKey = mqcfg["Queue"];
            var matchingGroup = _configuration["Service:ServiceIndex"];
            await _busControl.Publish(new SyncGameRoomMqCmd(gameKey, matchingGroup, null));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
