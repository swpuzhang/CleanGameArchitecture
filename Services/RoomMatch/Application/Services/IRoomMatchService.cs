using RoomMatch.ViewModels;
using Commons.DiIoc;
using Commons.Models;
using System.Threading.Tasks;
using GameMessages.MqCmds;
using GameMessages.MqEvents;

namespace RoomMatch.Application.Services
{
    public interface IRoomMatchService : IDependency
    {
        Task<WrappedResponse<RoomMatchResponseVm>> Playnow(long id);
        Task<WrappedResponse<GetBlindRoomListResponseVm>> GetBlindRoomList(long id);
        Task<WrappedResponse<RoomMatchResponseVm>> BlindMatch(long id, long blind);

        void SynGameRooms(SyncGameRoomMqCmd command);
        void OnJoinGameRoom(JoinGameRoomMqEvent joinEvent);
        void OnLeaveGameRoom(LeaveGameRoomMqEvent leaveEvent);
        Task<WrappedResponse<NullBody>> OnUserApplySit(UserApplySitMqCmd sitcmd);
        void OnUserSiteFailed(UserSitFailedMqEvent sitEvent);
    }
}
