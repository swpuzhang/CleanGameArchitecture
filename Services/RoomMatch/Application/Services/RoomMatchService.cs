using AutoMapper;
using Commons.Models;
using System.Threading.Tasks;
using Commons.Buses;
using RoomMatch.Manager;
using GameMessages.MqCmds;
using GameMessages.MqEvents;
using RoomMatch.ViewModels;
using RoomMatch.Domain.ProcessCommands;
using Commons.Enums;

namespace RoomMatch.Application.Services
{
    public class RoomMatchService : IRoomMatchService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        public RoomMatchService(IMapper mapper, IMediatorHandler bus)
        {
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<WrappedResponse<RoomMatchResponseVm>> Playnow(long id)
        {
            WrappedResponse<RoomMatchResponseVm> response = await _bus.SendCommand(new PlaynowCommand(id));
            return response.MapResponse<RoomMatchResponseVm>(_mapper);
        }

        public void SynGameRooms(SyncGameRoomMqCmd command)
        {
            RoomManager.SyncRooms(command.GameKey, command.MatchingGroup, command.SyncInfo);
        }

        public void OnJoinGameRoom(JoinGameRoomMqEvent joinEvent)
        {
            //_ = MatchManager.OnJoinGame(joinEvent.Id, joinEvent.GameKey, joinEvent.RoomId,
            //    joinEvent.Blind, joinEvent.UserCount, joinEvent.MatchingGroup);
        }

        public void OnLeaveGameRoom(LeaveGameRoomMqEvent leaveEvent)
        {
            _ = MatchManager.OnLeaveGame(leaveEvent.Id, leaveEvent.GameKey, leaveEvent.RoomId,
                leaveEvent.Blind, leaveEvent.UserCount, leaveEvent.MatchingGroup);
        }

        public Task<WrappedResponse<NullBody>> OnUserApplySit(UserApplySitMqCmd sitcmd)
        {
            return MatchManager.OnUserApplySit(sitcmd.Id, sitcmd.GameKey, sitcmd.Blind, sitcmd.RoomId);
        }

        public void OnUserSiteFailed(UserSitFailedMqEvent sitEvent)
        {
            _ = MatchManager.OnSiteFailed(sitEvent.Id, sitEvent.GameKey, sitEvent.RoomId, sitEvent.MatchingGroup);
        }

        public Task<WrappedResponse<GetBlindRoomListResponseVm>> GetBlindRoomList(long id)
        {
            return Task.FromResult(new WrappedResponse<GetBlindRoomListResponseVm>(ResponseStatus.Success, null,
                RoomManager.GetBindRoomList()));

        }

        public async Task<WrappedResponse<RoomMatchResponseVm>> BlindMatch(long id, long blind)
        {
            WrappedResponse<RoomMatchResponseVm> response =
                await _bus.SendCommand(new BlindMatchCommand(id, blind));
            return response.MapResponse<RoomMatchResponseVm>(_mapper);
        }

    }
}
