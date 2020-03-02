using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Models;
using Commons.DiIoc;

namespace WSGateWay.Services
{
    public interface ICommandService : IDependency
    {
        Task OnServerRequest(ConsumeContext<ServerRequest> context);
        Task OnGameRoomRequest(ConsumeContext<GameServerRequest> context);
    }
}
