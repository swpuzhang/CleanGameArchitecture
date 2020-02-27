using Game.Infrastruct.Repository;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Domain.ProcessEvents
{
    public class GameEventHandler : INotificationHandler<GameEvent>
    {
        /*private readonly IGameRedisRepository _GameRedisRep;
        private readonly IGameInfoRepository _GameRep;
        public GameEventHandler(IGameRedisRepository GameRedisRep, IGameInfoRepository GameRep)
        {
            _GameRedisRep = GameRedisRep;
            _GameRep = GameRep;
        }*/
        public Task Handle(GameEvent notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
