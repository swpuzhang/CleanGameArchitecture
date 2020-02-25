using AutoMapper;
using Commons.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WSGateway.Hubs;
using WSGateway.Manager;

namespace WSGateWay.Services
{
    public class CommandService : ICommandService
    {
        private readonly IHubContext<AppHub> _appHubContext;
        private readonly IMapper _mapper;
        public CommandService(IHubContext<AppHub> appHubContext, IMapper mapper)
        {

            _appHubContext = appHubContext;
            _mapper = mapper;
        }

        public Task OnGameRoomRequest(ConsumeContext<GameServerRequest> context)
        {
            GameServerRequest serverReq = context.Message;
            string conn = UserConnManager.GetConnByUid(serverReq.Id);
            if (conn == null)
            {
                return Task.CompletedTask;
            }
            ToAppRoomRequest req = _mapper.Map<ToAppRoomRequest>(serverReq);
            return _appHubContext.Clients.Clients(conn).SendAsync("ToAppRoomRequest", req);
        }

        public  Task OnServerRequest(ConsumeContext<ServerRequest> context)
        {
            ServerRequest serverReq = context.Message;
            string conn = UserConnManager.GetConnByUid(serverReq.Id);
            if (conn == null)
            {
                return Task.CompletedTask;
            }
            ToAppRequest req = _mapper.Map<ToAppRequest>(context.Message);
            return _appHubContext.Clients.Clients(conn).SendAsync("ToAppRequest", req);
            //var response = await _rpcCaller.RequestCallAsync(conn, "ToAppRequest", JsonConvert.SerializeObject(req), req.MessageId);
            //await context.RespondAsync<BodyResponse<NullBody>>(response);
        }
    }
}
