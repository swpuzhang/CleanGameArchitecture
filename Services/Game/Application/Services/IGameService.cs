using Game.ViewModels;
using Commons.DiIoc;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMessages.MqCmds;

namespace Game.Application.Services
{
    public interface IGameService : IDependency
    {
        Task<WrappedResponse<NullBody>> CreatRoom(CreateRoomMqCmd creatInfo);
        Task<WrappedResponse<JoinGameRoomMqResponse>> JoinRoom(JoinGameRoomMqCmd joinInfo);

        Task<ToAppResponse> GameRoomMessage(AppRoomRequest request);

        void MatchingStarted(string MatchingGroup);
    }
}
