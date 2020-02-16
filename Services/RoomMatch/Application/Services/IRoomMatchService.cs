using RoomMatch.ViewModels;
using Commons.DiIoc;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Commons.Buses;

namespace RoomMatch.Application.Services
{
    public interface IRoomMatchService : IDependency
    {
        Task<WrappedResponse<RoomMatchResponseVm>> Playnow(long id);
        Task<WrappedResponse<GetBlindRoomListResponseVm>> GetBlindRoomList(long id);
        Task<WrappedResponse<RoomMatchResponseVm>> BlindMatching(long id, long blind);

        void SynGameRooms(SyncGameRoomMqCommand command);
        void OnJoinGameRoom(JoinGameRoomMqEvent joinEvent);
        void OnLeaveGameRoom(LeaveGameRoomMqEvent leaveEvent);
        Task<WrappedResponse<NullBody>> OnUserApplySit(UserApplySitMqCommand sitcmd);
        void OnUserSiteFailed(UserSitFailedMqEvent sitEvent);
    }
}
