using Commons.Buses.MqBus;
using Commons.Enums;
using Commons.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSGateway.Manager;
using WSGateway.ViewModels;
using WSGateWay.Services;

namespace WSGateway.Hubs
{
    public class AppHub : Hub
    {
        private readonly ICommonService _commonService;
        private readonly IBusControl _bus;
        private readonly IConfiguration Configuration;
        public AppHub(//IRequestClient<AppRoomRequest> requestClient,
            ICommonService commonService,
            IBusControl bus, IConfiguration configuration)
        {
            //_requestClient = requestClient;
            _commonService = commonService;
            _bus = bus;
            Configuration = configuration;
        }

      
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            UserConnManager.OnDisconnected(Context.ConnectionId);
        }

        public ToAppResponse TestRequest()
        {

            return new ToAppResponse(null, ResponseStatus.Success, null);
        }

        public WrappedResponse<NullBody> LoginRequest(LoginRequest request)
        {
            //验证token是否有效
            //如果有效将创建uid和玩家对应的关系
            var result = _commonService.TokenValidation(request.Token);
            if (!result.Key)
            {
                return new WrappedResponse<NullBody>(ResponseStatus.LoginError, new List<string>() { "Token error relogin" });
            }
            UserConnManager.OnLogined(result.Value, Context.ConnectionId);

            return new WrappedResponse<NullBody>(ResponseStatus.Success, null);

        }

        public async Task<ToAppResponse> AppRoomRequest(AppRoomRequest request)
        {
            ToAppResponse commonResponse;

            //验证是否是本人ID
            long uid = UserConnManager.GetUidByConn(Context.ConnectionId);
            if (request.Id != uid)
            {
                return new ToAppResponse(null, ResponseStatus.Error, null);
            }
            var busClient = _bus.CreateRequestClient<AppRoomRequest>(new Uri($"{Configuration["Rabbitmq:Uri"]}{request.GameRoomKey}"), TimeSpan.FromSeconds(5));
            try
            {
                var busResponse = await busClient.GetResponseExt<AppRoomRequest, ToAppResponse>(request);
                commonResponse = busResponse?.Message;
            }
            catch (Exception)
            {

                return new ToAppResponse(null, ResponseStatus.BusError, null);
            }
            return commonResponse;
        }
    }
}
