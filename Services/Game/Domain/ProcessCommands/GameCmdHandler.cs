using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Game.ViewModels;
using Commons.Models;
using MediatR;
using Game.Infrastruct.Repository;
using Game.Domain.Entitys;
using Commons.Tools.Encryption;
using Commons.Buses;
using Game.Domain.ProcessEvents;
using Commons.Enums;
using CommonMessages.MqCmds;
using MassTransit;
using Commons.Buses.MqBus;
using Serilog;

namespace Game.Domain.ProcessCommands
{
    public class GameCmdHandler : IRequestHandler<GameCommand, WrappedResponse<GameResponseVm>>
    {
        
        /*private readonly IGameRedisRepository _GameRedisRep;
        private readonly IGameInfoRepository _GameRep;
        private readonly IMediatorHandler _bus;

        public GameCmdHandler(IGameRedisRepository GameRedisRep, IGameInfoRepository GameRep,
            IMediatorHandler bus)
        {
            _GameRedisRep = GameRedisRep;
            _GameRep = GameRep;
            _bus = bus;
        }*/

        public  Task<WrappedResponse<GameResponseVm>> Handle(GameCommand request, CancellationToken cancellationToken)
        {
            
            WrappedResponse<GameResponseVm> response = new WrappedResponse<GameResponseVm>(ResponseStatus.LoginError,
               null, null);

            return Task.FromResult(response);
        }
    }
}
