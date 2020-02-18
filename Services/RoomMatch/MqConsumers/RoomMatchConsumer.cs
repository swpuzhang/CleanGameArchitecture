using MassTransit;
using CommonMessages.MqCmds;
using RoomMatch.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Threading;
using GameMessages.MqCmds;
using GameMessages.MqEvents;
using Commons.Models;

namespace Money.MqConsumers
{
    public class RoomMatchConsumer :
         OneThreadConsumer<SyncGameRoomMqCmd>
    {
        private IRoomMatchService _service;


        public RoomMatchConsumer(IRoomMatchService service)
        {
            _service = service;
        }

        public override void ConsumerHandler(SyncGameRoomMqCmd request)
        {
            _service.SynGameRooms(request);
        }
    }

    public class JoinGameRoomConsumer :
       OneThreadConsumer<JoinGameRoomMqEvent>
    {
        IRoomMatchService _service;

        public JoinGameRoomConsumer(IRoomMatchService service)
        {
            _service = service;
        }

        public override void ConsumerHandler(JoinGameRoomMqEvent request)
        {
            _service.OnJoinGameRoom(request);
        }
    }

    public class LeaveGameRoomConsumer :
        OneThreadConsumer<LeaveGameRoomMqEvent>
    {
        IRoomMatchService _service;

        public LeaveGameRoomConsumer(IRoomMatchService service)
        {
            _service = service;
        }

        public override void ConsumerHandler(LeaveGameRoomMqEvent request)
        {
            _service.OnLeaveGameRoom(request);
        }
    }


    public class UserApplySitConsumer :
        OneThreadConsumer<UserApplySitMqCmd, WrappedResponse<NullBody>>
    {
        IRoomMatchService _service;

        public UserApplySitConsumer(IRoomMatchService service)
        {
            _service = service;
        }


        public async override Task<WrappedResponse<NullBody>> ConsumerHandler(UserApplySitMqCmd request)
        {
            return await _service.OnUserApplySit(request);
        }
    }

    public class UserSitFailedConsumer :
       OneThreadConsumer<UserSitFailedMqEvent>
    {
        IRoomMatchService _service;

        public UserSitFailedConsumer(IRoomMatchService service)
        {
            _service = service;
        }




        public override void ConsumerHandler(UserSitFailedMqEvent request)
        {
            _service.OnUserSiteFailed(request);
        }
    }
}
