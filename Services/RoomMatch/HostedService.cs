using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RoomMatch.Infrastruct.Repository;
using RoomMatch.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RoomMatch
{
    public class HostedService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;
        private readonly IRoomListConfigRepository _roomRep;
        private readonly ICoinsRangeConfigRepository _coinRangeRep;
        private readonly IRoomMatchRedisRepository _matchRep;

        public HostedService(IBusControl busControl, IConfiguration configuration, 
            IRoomListConfigRepository roomRep, ICoinsRangeConfigRepository coinRangeRep, 
            IRoomMatchRedisRepository matchRep)
        {
            _busControl = busControl;
            _configuration = configuration;
            _roomRep = roomRep;
            _coinRangeRep = coinRangeRep;
            _matchRep = matchRep;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            MatchManager.Init(_configuration, _coinRangeRep, _matchRep);
            RoomManager.Init(_busControl, _configuration, _roomRep);
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
