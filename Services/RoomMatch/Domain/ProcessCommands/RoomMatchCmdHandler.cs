using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RoomMatch.ViewModels;
using Commons.Models;
using MediatR;
using RoomMatch.Infrastruct.Repository;
using RoomMatch.Domain.Entitys;
using Commons.Tools.Encryption;
using Commons.Buses;
using RoomMatch.Domain.ProcessEvents;
using Commons.Enums;
using CommonMessages.MqCmds;
using MassTransit;
using Commons.Buses.MqBus;
using Serilog;

namespace RoomMatch.Domain.ProcessCommands
{
    public class RoomMatchCmdHandler : IRequestHandler<RoomMatchCommand, WrappedResponse<RoomMatchResponseVm>>
    {
        
        /*private readonly IRoomMatchRedisRepository _RoomMatchRedisRep;
        private readonly IRoomMatchInfoRepository _RoomMatchRep;
        private readonly IMediatorHandler _bus;

        public RoomMatchCmdHandler(IRoomMatchRedisRepository RoomMatchRedisRep, IRoomMatchInfoRepository RoomMatchRep,
            IMediatorHandler bus)
        {
            _RoomMatchRedisRep = RoomMatchRedisRep;
            _RoomMatchRep = RoomMatchRep;
            _bus = bus;
        }*/

        public  Task<WrappedResponse<RoomMatchResponseVm>> Handle(RoomMatchCommand request, CancellationToken cancellationToken)
        {
            
            WrappedResponse<RoomMatchResponseVm> response = new WrappedResponse<RoomMatchResponseVm>(ResponseStatus.LoginError,
               null, null);

            return Task.FromResult(response);
        }
    }
}
