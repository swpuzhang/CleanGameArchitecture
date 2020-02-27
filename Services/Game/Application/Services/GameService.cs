using Game.ViewModels;
using AutoMapper;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Buses.ProcessBus;
using Commons.Buses;
using Game.Domain.ProcessCommands;
using Game.Domain.Entitys;
using GameMessages.MqCmds;
using Game.Domain.Manager;
using Newtonsoft.Json;
using GameMessages.RoomMessages;

namespace Game.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly GameRoomManager _gameRoomManager;

        public GameService(IMapper mapper,
            GameRoomManager gameRoomManager)
        {
            _mapper = mapper;
            _gameRoomManager = gameRoomManager;
        }

        public Task<WrappedResponse<NullBody>> CreatRoom(CreateRoomMqCmd creatInfo)
        {
            return Task.FromResult(_gameRoomManager.CreateRoom(creatInfo.RoomType, creatInfo.RoomId, creatInfo.Blind, creatInfo.SeatCount,
               creatInfo.MaxCoins, creatInfo.MaxCoins, creatInfo.TipsPersent, creatInfo.MinCarry, creatInfo.MaxCarry));
        }

        public Task<ToAppResponse> GameRoomMessage(AppRoomRequest request)
        {
            Type t = Type.GetType($"{RoomMessage.MessageNameSpace}.{request.ReqName}");
            var obj = JsonConvert.DeserializeObject(request.ReqData, t);
            return Task.FromResult(_gameRoomManager.OnRoomRequest(request.Id, request.RoomId, request.ReqName, obj));
        }


        public Task<WrappedResponse<JoinGameRoomMqResponse>> JoinRoom(JoinGameRoomMqCmd joinInfo)
        {
            return _gameRoomManager.JoinRoom(joinInfo.Id, joinInfo.RoomId);
        }

        public void MatchingStarted(string MatchingGroup)
        {
            _gameRoomManager.MatchingStarted(MatchingGroup);
        }
    }
}
