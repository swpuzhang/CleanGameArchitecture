using MassTransit;
using CommonMessages.MqCmds;
using Game.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Threading;
using GameMessages.MqCmds;
using Commons.Models;
using GameMessages.MqEvents;

namespace Money.MqConsumers
{
    public class CreateRooConsumer :
        OneThreadConsumer<CreateRoomMqCmd, WrappedResponse<NullBody>>
    {

        private readonly IGameService _service;
        public CreateRooConsumer(IGameService service)
        {
            _service = service;
        }

        public override Task<WrappedResponse<NullBody>> ConsumerHandler(CreateRoomMqCmd request)
        {
            return _service.CreatRoom(request);
        }
    }

    public class JoinRoomConsummer : OneThreadConsumer<JoinGameRoomMqCmd, WrappedResponse<JoinGameRoomMqResponse>>
    {
        private readonly IGameService _service;
        public JoinRoomConsummer(IGameService service)
        {
            _service = service;
        }
        public override Task<WrappedResponse<JoinGameRoomMqResponse>> ConsumerHandler(JoinGameRoomMqCmd request)
        {
            return _service.JoinRoom(request);
        }
    }

    public class GameRoomConsummer : OneThreadConsumer<AppRoomRequest, ToAppResponse>
    {
        private readonly IGameService _service;
        public GameRoomConsummer(IGameService service)
        {
            _service = service;
        }
        public override async Task<ToAppResponse> ConsumerHandler(AppRoomRequest request)
        {
            return await _service.GameRoomMessage(request);
        }
    }

    public class MatchingStartedConsummer : OneThreadConsumer<MatchingStartedEvent>
    {
        private readonly IGameService _service;
        public MatchingStartedConsummer(IGameService service)
        {
            _service = service;
        }
        public override void ConsumerHandler(MatchingStartedEvent request)
        {
            _service.MatchingStarted(request.MatchingGroup);
        }
    }
}
