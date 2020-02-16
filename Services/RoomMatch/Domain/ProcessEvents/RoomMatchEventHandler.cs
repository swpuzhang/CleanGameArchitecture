using RoomMatch.Infrastruct.Repository;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RoomMatch.Domain.ProcessEvents
{
    public class RoomMatchEventHandler : INotificationHandler<RoomMatchEvent>
    {
        /*private readonly IRoomMatchRedisRepository _RoomMatchRedisRep;
        private readonly IRoomMatchInfoRepository _RoomMatchRep;
        public RoomMatchEventHandler(IRoomMatchRedisRepository RoomMatchRedisRep, IRoomMatchInfoRepository RoomMatchRep)
        {
            _RoomMatchRedisRep = RoomMatchRedisRep;
            _RoomMatchRep = RoomMatchRep;
        }*/
        public Task Handle(RoomMatchEvent notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
