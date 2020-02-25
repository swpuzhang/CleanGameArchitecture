using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Models;

namespace WSGateWay.Services
{
    public interface ICommandService
    {
        Task OnServerRequest(ConsumeContext<ServerRequest> context);
        Task OnGameRoomRequest(ConsumeContext<GameServerRequest> context);
    }
}
